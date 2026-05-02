# ADR 0011 - Usar GitHub Actions para CI de backend e frontend

## Status

Aceita

## Contexto

O projeto SpecPilot AI evoluiu para um MVP com backend .NET 8 e frontend React + TypeScript com testes automatizados.
Ja existia um workflow de CI focado apenas no backend.
Com a adicao dos testes frontend, passou a ser necessario validar ambos os lados automaticamente em `push` e `pull_request`, sem aumentar escopo com deploy.

## Decisao

Adotar um workflow unico em `.github/workflows/ci.yml` com dois jobs separados:

- `backend`: restore, build e testes da solution `src/backend/SpecPilot.sln`
- `frontend`: `npm ci`, `npm run build` e `npm test` em `src/frontend/specpilot-web`

Regras operacionais da decisao:

- nao executar deploy nesta etapa
- nao publicar imagem Docker
- nao usar segredos reais para OpenAI
- fixar `Ai__Provider=Fake` no job de backend para impedir chamadas reais ao provider externo
- manter testes frontend isolados de backend real (mocks locais)

`backend-ci.yml` foi removido para evitar duplicidade de pipelines.

## Consequencias positivas

- validacao automatica ponta a ponta do codigo do MVP (backend + frontend)
- reducao de regressao em pull requests
- pipeline simples e didatica para contexto academico
- rastreabilidade mais clara com um unico ponto de entrada de CI

## Consequencias negativas

- tempo total de CI maior do que no fluxo apenas backend
- maior manutencao do workflow unico conforme evolucao das stacks

## Alternativas consideradas

1. Manter apenas CI de backend:
   - descartada por nao cobrir regressao no frontend.

2. Criar dois workflows separados (`backend-ci.yml` e `frontend-ci.yml`):
   - possivel, mas gera mais dispersao para o contexto atual.
   - preferimos `ci.yml` unico com jobs separados por simplicidade e visibilidade.

3. Adicionar deploy no CI:
   - descartada por estar fora do escopo do MVP desta fase.
