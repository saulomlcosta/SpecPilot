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

### Refinamento

- `POST /api/projects/{id}/refinement-questions`
- `POST /api/projects/{id}/refinement-answers`

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
  "initialDescription": "Quero um sistema para agendamento, prontuario e notificacoes."
}
```

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
