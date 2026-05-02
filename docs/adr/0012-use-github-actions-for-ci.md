# 0012 - Use GitHub Actions para CI

## Status

Aceita

## Contexto

O projeto SpecPilot AI ja possui backend em .NET 8, testes unitarios e testes de integracao consolidados para o fluxo principal do MVP. Apos o Prompt 12, tornou-se importante validar automaticamente restore, build e testes a cada push e pull request, reduzindo risco de regressao e reforcando a confiabilidade academica do projeto.

Os testes atuais do backend usam `FakeAiService` e, na configuracao de integracao, utilizam banco em memoria por meio de `WebApplicationFactory`. Por isso, a pipeline nao precisa chamar OpenAI real nem subir PostgreSQL manualmente neste momento.

## Decisao

Adotar GitHub Actions como pipeline de integracao continua do repositorio, com um workflow simples para o backend executado em `ubuntu-latest`.

O workflow deve:

- rodar em `push` e `pull_request`
- executar `dotnet restore`
- executar `dotnet build --no-restore --configuration Release`
- executar `dotnet test --no-build --configuration Release`
- usar a solution `src/backend/SpecPilot.sln`
- fixar `Ai__Provider=Fake`

Como os testes de integracao atuais usam banco em memoria, nao sera configurado service container PostgreSQL nesta etapa.

## Consequencias positivas

- validacao automatica do backend em cada alteracao relevante
- deteccao mais rapida de regressao em build e testes
- reforco da previsibilidade do MVP em contexto academico
- nenhuma dependencia de segredos reais para rodar a pipeline
- manutencao do `FakeAiService` como comportamento padrao no CI

## Consequencias negativas

- aumento do tempo total de verificacao em cada push e pull request
- necessidade de manter o workflow atualizado se a estrutura da solution mudar
- futura migracao dos testes de integracao para PostgreSQL real pode exigir ajuste da pipeline

## Alternativas consideradas

- nao usar CI nesta etapa e depender apenas de execucao local
- usar outra plataforma de CI, como Azure Pipelines ou GitLab CI
- configurar PostgreSQL manualmente no workflow desde agora

As alternativas foram descartadas porque aumentariam custo operacional ou complexidade sem ganho proporcional para o MVP atual.
