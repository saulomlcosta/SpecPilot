# 08 - Estrategia de testes

## Objetivo

Garantir qualidade, seguranca de mudancas e execucao previsivel em ambiente local e academico.

## Tipos de teste previstos

### Testes unitarios

Devem cobrir:

- validacoes
- regras de negocio
- transformacoes de dados
- comportamentos do servico de IA fake
- selecao do provider de IA por configuracao
- protecao do fluxo de `ProjectStatus` nos casos de uso

### Testes de integracao

Devem cobrir:

- endpoints principais
- integracao com banco de dados
- fluxo principal do MVP de ponta a ponta:
  - registro
  - login
  - criacao de projeto
  - geracao de perguntas
  - resposta das perguntas
  - geracao de documento
  - consulta do documento gerado
- respostas `ProblemDetails` para erros esperados
- tratamento global de excecoes inesperadas

## Diretrizes

- cada caso de uso relevante deve ter cobertura proporcional
- testes nao devem depender de internet
- `FakeAiService` deve ser o padrao para testes
- o ambiente de integracao deve fixar explicitamente `Ai__Provider=Fake`
- cenarios felizes e invalidos devem ser cobertos
- cenarios de erro HTTP devem validar status e formato padronizado da resposta
- verificacoes de persistencia devem preferir consultas `AsNoTracking()` e chamadas assincronas
- os testes do backend tambem devem ser executados automaticamente no CI

### Testes de frontend

Devem cobrir, de forma enxuta e orientada a comportamento:

- formularios principais (login, cadastro, criacao de projeto)
- acoes guiadas por `ProjectStatus` no detalhe do projeto
- renderizacao do documento tecnico no frontend
- tratamento amigavel de erros `ProblemDetails`
- helpers essenciais, como armazenamento de token

Stack adotada no frontend:

- Vitest
- React Testing Library
- jsdom
- `@testing-library/jest-dom`
- `@testing-library/user-event`

Diretrizes especificas:

- mockar chamadas HTTP e servicos para evitar dependencia de backend real
- nao depender de Docker para executar testes de frontend
- nao realizar chamadas reais a OpenAI
- priorizar testes pequenos de alto valor em vez de cobertura extensa e fragil

## Aplicacao ao servico de IA

- a abstracao de IA deve ter testes unitarios proprios
- o `FakeAiService` deve retornar respostas deterministicas
- a selecao do provider deve poder ser verificada por configuracao
- nenhum teste desta etapa deve depender de credenciais externas
- nenhum teste desta etapa deve realizar chamada real a OpenAI

## Beneficio didatico

Essa estrategia ajuda a demonstrar separacao de responsabilidades e confiabilidade sem exigir infraestrutura externa complexa.

## Aplicacao no CI

O repositorio usa GitHub Actions em `.github/workflows/ci.yml` para validar backend e frontend em cada `push` e `pull_request`.

No backend, o CI executa:

- `dotnet restore src/backend/SpecPilot.sln`
- `dotnet build src/backend/SpecPilot.sln --no-restore --configuration Release`
- `dotnet test src/backend/SpecPilot.sln --no-build --configuration Release`

No frontend, o CI executa:

- `npm ci`
- `npm run build`
- `npm test` (modo nao interativo via `vitest run`)

Decisoes importantes desta etapa:

- testes frontend rodam isolados, sem depender de backend real
- testes backend mantem `Ai__Provider=Fake` para impedir chamadas reais a OpenAI
- nao ha deploy, publicacao de imagem ou uso de segredos reais no CI
- nao ha necessidade de subir PostgreSQL manualmente no workflow para os testes atuais do backend
