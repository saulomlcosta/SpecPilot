# Validacao manual do backend

## Objetivo

Fornecer um roteiro curto para validar manualmente o fluxo principal do MVP antes de iniciar o frontend.

## Pre-requisitos

- Docker ativo
- `.env` criado a partir de `.env.example`
- `Ai__Provider=Fake`

## Subida do ambiente

No PowerShell:

```powershell
Copy-Item .env.example .env
docker compose down -v
docker compose up --build
```

Resultados esperados:

- API em `http://localhost:8080`
- Swagger em `http://localhost:8080/swagger`
- health check em `http://localhost:8080/health`

## Fluxo principal do MVP

### 1. Registrar usuario

`POST /api/auth/register`

```json
{
  "name": "Saulo",
  "email": "saulo@example.com",
  "password": "12345678"
}
```

### 2. Fazer login

`POST /api/auth/login`

```json
{
  "email": "saulo@example.com",
  "password": "12345678"
}
```

Copie o `token` retornado.

### 3. Confirmar autenticacao

`GET /api/auth/me`

Header:

```text
Authorization: Bearer <token>
```

### 4. Criar projeto

`POST /api/projects`

```json
{
  "name": "Projeto de validacao",
  "initialDescription": "Sistema para validar o fluxo principal do MVP.",
  "goal": "Confirmar prontidao do backend para o frontend.",
  "targetAudience": "Avaliadores e estudantes"
}
```

Resultado esperado:

- status inicial `Draft`

### 5. Gerar perguntas

`POST /api/projects/{id}/generate-questions`

Resultado esperado:

- status `QuestionsGenerated`
- lista de perguntas retornada
- provider efetivo `FakeAiService` quando `Ai__Provider=Fake`

### 5.1. Buscar perguntas geradas

`GET /api/projects/{id}/questions`

Resultado esperado:

- lista de perguntas com `id`, `order`, `questionText` e `answer`
- uso desses `questionId` no passo de resposta

### 6. Responder perguntas

`PUT /api/projects/{id}/questions/answers`

```json
{
  "answers": [
    {
      "questionId": "guid",
      "answer": "Resposta 1"
    }
  ]
}
```

Resultado esperado:

- todas as perguntas devem ser respondidas na mesma requisicao
- status `QuestionsAnswered`

### 7. Gerar documento

`POST /api/projects/{id}/generate-document`

Resultado esperado:

- status `DocumentGenerated`
- secoes `overview`, `functionalRequirements`, `nonFunctionalRequirements`, `useCases` e `risks`

### 8. Buscar documento

`GET /api/projects/{id}/document`

Resultado esperado:

- retorno do documento persistido

## Erros importantes para validar

### Endpoint protegido sem token

`GET /api/projects`

Resultado esperado:

- `401 Unauthorized`
- `ProblemDetails`

### Gerar documento antes de responder perguntas

Crie um projeto novo e chame:

`POST /api/projects/{id}/generate-document`

Resultado esperado:

- `409 Conflict`
- codigo `projects.invalid_status_for_document_generation`

### Acessar projeto de outro usuario

Autentique um segundo usuario e tente:

`GET /api/projects/{id}`

Resultado esperado:

- `404 Not Found`
- codigo `projects.not_found`

## Confirmacoes finais

- nenhuma chamada real a OpenAI deve ocorrer com `Ai__Provider=Fake`
- o frontend nao e necessario para validar o backend nesta etapa
