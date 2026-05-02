# 07 - Contratos iniciais de API

## Objetivo

Este documento descreve contratos iniciais para orientar a implementacao da Web API do SpecPilot AI.

## Endpoints previstos

### Autenticacao

- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/auth/me`

### Projetos

- `POST /api/projects`
- `GET /api/projects`
- `GET /api/projects/{id}`
- `PUT /api/projects/{id}`
- `DELETE /api/projects/{id}`

### Refinamento

- `POST /api/projects/{id}/generate-questions`
- `PUT /api/projects/{id}/questions/answers`

### Documento

- `POST /api/projects/{id}/initial-document`
- `GET /api/projects/{id}/initial-document`

## Contratos de autenticacao

### `POST /api/auth/register`

Request:

```json
{
  "name": "Saulo",
  "email": "saulo@example.com",
  "password": "12345678"
}
```

Response `201 Created`:

```json
{
  "token": "jwt-token",
  "user": {
    "id": "guid",
    "name": "Saulo",
    "email": "saulo@example.com"
  }
}
```

### `POST /api/auth/login`

Request:

```json
{
  "email": "saulo@example.com",
  "password": "12345678"
}
```

Response `200 OK`:

```json
{
  "token": "jwt-token",
  "user": {
    "id": "guid",
    "name": "Saulo",
    "email": "saulo@example.com"
  }
}
```

### `GET /api/auth/me`

Header:

```text
Authorization: Bearer <jwt-token>
```

Response `200 OK`:

```json
{
  "id": "guid",
  "name": "Saulo",
  "email": "saulo@example.com"
}
```

Response `401 Unauthorized` quando o token estiver ausente ou invalido.

## Padrao de erros HTTP

As falhas esperadas da API devem retornar `application/problem+json` usando `ProblemDetails`.

Campos esperados:

- `title`: resumo curto do erro
- `detail`: descricao legivel do problema
- `status`: codigo HTTP
- `code`: codigo interno do erro no campo `extensions`

Exemplo de erro de validacao:

```json
{
  "title": "Requisicao invalida.",
  "detail": "O email informado e invalido. A senha deve ter pelo menos 8 caracteres.",
  "status": 400,
  "code": "common.validation_error"
}
```

Exemplo de conflito de cadastro:

```json
{
  "title": "Conflito de negocio.",
  "detail": "Ja existe um usuario cadastrado com este email.",
  "status": 409,
  "code": "auth.email_already_registered"
}
```

Exemplo de credenciais invalidas:

```json
{
  "title": "Nao autorizado.",
  "detail": "Email ou senha invalidos.",
  "status": 401,
  "code": "auth.invalid_credentials"
}
```

## Exemplo conceitual de criacao de projeto

```json
{
  "name": "Sistema de clinica",
  "initialDescription": "Quero um sistema para agendamento, prontuario e notificacoes.",
  "goal": "Organizar o atendimento da clinica.",
  "targetAudience": "Equipe administrativa e medica"
}
```

## Contratos de projetos

### `POST /api/projects`

Header:

```text
Authorization: Bearer <jwt-token>
```

Request:

```json
{
  "name": "Sistema de clinica",
  "initialDescription": "Quero um sistema para agendamento, prontuario e notificacoes.",
  "goal": "Organizar o atendimento da clinica.",
  "targetAudience": "Equipe administrativa e medica"
}
```

Response `201 Created`:

```json
{
  "id": "guid",
  "name": "Sistema de clinica",
  "initialDescription": "Quero um sistema para agendamento, prontuario e notificacoes.",
  "goal": "Organizar o atendimento da clinica.",
  "targetAudience": "Equipe administrativa e medica",
  "status": "Draft",
  "createdAt": "2026-05-01T12:00:00Z",
  "updatedAt": null
}
```

### `GET /api/projects`

Response `200 OK`:

```json
[
  {
    "id": "guid",
    "name": "Sistema de clinica",
    "initialDescription": "Quero um sistema para agendamento, prontuario e notificacoes.",
    "goal": "Organizar o atendimento da clinica.",
    "targetAudience": "Equipe administrativa e medica",
    "status": "Draft",
    "createdAt": "2026-05-01T12:00:00Z",
    "updatedAt": null
  }
]
```

### `GET /api/projects/{id}`

Response `200 OK` com o mesmo formato de projeto.

Response `404 Not Found` quando o projeto nao existir ou nao pertencer ao usuario autenticado.

### `PUT /api/projects/{id}`

Request:

```json
{
  "name": "Sistema de clinica atualizado",
  "initialDescription": "Descricao refinada da ideia.",
  "goal": "Melhorar a organizacao do atendimento.",
  "targetAudience": "Equipe administrativa, medica e recepcao"
}
```

Response `200 OK` com o projeto atualizado.

Observacoes:

- o endpoint atualiza apenas campos editaveis do projeto
- `status` nao faz parte do contrato de update comum
- tentativas de enviar `status` no JSON devem ser ignoradas pelo model binding
- `ProjectStatus` muda apenas nos casos de uso especificos do fluxo

### `DELETE /api/projects/{id}`

Response `204 No Content`.

Response `404 Not Found` quando o projeto nao existir ou nao pertencer ao usuario autenticado.

### `POST /api/projects/{id}/generate-questions`

Header:

```text
Authorization: Bearer <jwt-token>
```

Response `200 OK`:

```json
{
  "projectId": "guid",
  "status": "QuestionsGenerated",
  "questions": [
    "Quem sao os principais usuarios do sistema?",
    "Quais funcionalidades sao essenciais para a primeira versao?"
  ]
}
```

Regras do endpoint:

- apenas o dono do projeto pode gerar perguntas
- o projeto precisa estar com status `Draft`
- as perguntas geradas devem ser persistidas
- o status do projeto deve mudar para `QuestionsGenerated`
- a interacao com IA deve ser registrada em log

Response `404 Not Found` quando o projeto nao existir ou nao pertencer ao usuario autenticado.

Response `409 Conflict` quando o projeto nao estiver em status `Draft`.

### `PUT /api/projects/{id}/questions/answers`

Header:

```text
Authorization: Bearer <jwt-token>
```

Request:

```json
{
  "answers": [
    {
      "questionId": "guid",
      "answer": "Analistas e gestores."
    },
    {
      "questionId": "guid",
      "answer": "Refinar requisitos do produto."
    }
  ]
}
```

Response `200 OK`:

```json
{
  "projectId": "guid",
  "status": "QuestionsAnswered"
}
```

Regras do endpoint:

- apenas o dono do projeto pode responder as perguntas
- o projeto precisa estar com status `QuestionsGenerated`
- todas as perguntas do projeto devem ser respondidas na mesma requisicao
- as respostas devem ser persistidas nas perguntas de refinamento
- o status do projeto deve mudar para `QuestionsAnswered`

Response `400 Bad Request` quando a requisicao nao enviar todas as respostas obrigatorias.

Response `404 Not Found` quando o projeto nao existir ou nao pertencer ao usuario autenticado.

Response `409 Conflict` quando o projeto nao estiver em status `QuestionsGenerated` ou ainda nao possuir perguntas geradas.

## Exemplo conceitual de documento gerado

```json
{
  "overview": "Sistema web para apoiar a operacao de uma clinica.",
  "functionalRequirements": [
    "Permitir cadastro de pacientes",
    "Permitir agendamento de consultas"
  ],
  "nonFunctionalRequirements": [
    "Disponibilidade em horario comercial",
    "Controle basico de acesso"
  ],
  "useCases": [
    "Cadastrar paciente",
    "Agendar consulta"
  ],
  "risks": [
    "Descricao inicial insuficiente",
    "Requisitos ambiguos"
  ]
}
