# Release Notes — SpecPilot AI MVP

## Versao

- MVP 1.0

## Data

- 2026-05-02

## Resumo da entrega

Esta release registra a entrega final do MVP academico do SpecPilot AI. O projeto entrega um fluxo completo para transformar uma ideia inicial de software em um documento tecnico inicial, com apoio de IA Generativa, backend e frontend funcionais, ambiente reprodutivel com Docker Compose, testes automatizados, CI com GitHub Actions e documentacao versionada.

## Funcionalidades implementadas

- cadastro de usuario;
- login;
- logout;
- gerenciamento de projetos;
- geracao de perguntas de refinamento;
- resposta das perguntas;
- geracao de documento tecnico;
- visualizacao do documento.

## Uso de IA Generativa

O MVP usa IA Generativa em dois pontos do produto:

- geracao de perguntas de refinamento;
- geracao do documento tecnico inicial.

Aspectos tecnicos relevantes:

- `FakeAiService` e o provider padrao para execucao local e testes;
- OpenAI e opcional por configuracao;
- os prompts runtime seguem o metodo CO-STAR;
- as respostas da IA sao solicitadas em JSON estruturado;
- o backend valida o formato da resposta antes de seguir no fluxo;
- `AiInteractionLog` apoia a rastreabilidade das interacoes com IA;
- a integracao com IA fica centralizada no backend;
- o frontend nao chama OpenAI diretamente.

## Uso de IA no desenvolvimento

O desenvolvimento do projeto tambem usou IA de forma controlada e documentada:

- Codex foi utilizado como agente auxiliar de desenvolvimento;
- o trabalho foi dividido em prompts pequenos e versionados em `prompts/codex/`;
- o processo foi conduzido com human-in-the-loop;
- o `docs/development-log.md` registrou marcos, problemas e retomadas;
- as ADRs registraram decisoes arquiteturais;
- os commits seguiram Conventional Commits;
- skills e superpowers como `brainstorming`, `systematic-debugging`, `test-driven-development` e `verification-before-completion` foram usadas como apoio metodologico quando aplicavel.

Essa abordagem reforcou rastreabilidade, controle de escopo e reproducibilidade do processo academico.

## Entregas tecnicas

- backend .NET;
- frontend React;
- PostgreSQL;
- Docker Compose;
- CI com GitHub Actions;
- testes backend;
- testes frontend;
- documentacao em `docs/`;
- ADRs;
- prompts runtime;
- prompts Codex.

## Testes e validacao

Esta release foi validada por meio de:

- `dotnet test src/backend/SpecPilot.sln`;
- `npm ci`;
- `npm run build`;
- `npm test`;
- validacao de backend e frontend no CI;
- validacao do `docker compose`;
- validacao do fluxo principal do MVP.

O conjunto dessas verificacoes busca garantir que a entrega final seja executavel, testavel e coerente com o escopo academico definido.

## Decisoes arquiteturais relevantes

Principais decisoes consolidadas nesta release:

- arquitetura em camadas;
- PostgreSQL como banco relacional;
- Result Pattern + `ProblemDetails`;
- Global Exception Handler;
- `FakeAiService` como provider padrao;
- provider OpenAI opcional via `HttpClient`;
- protecao das transicoes de `ProjectStatus`;
- Docker Compose como ambiente local principal;
- GitHub Actions para validacao continua;
- prompts runtime em CO-STAR.

## Limitacoes conhecidas

- sem RAG;
- sem upload;
- sem PDF;
- sem chat livre;
- sem multiplos agentes;
- sem deploy cloud;
- sem colaboracao multiusuario;
- sem Playwright E2E completo.

## Proximos passos possiveis

- RAG;
- upload de arquivos;
- exportacao PDF;
- versionamento de documentos;
- multiplos agentes especializados;
- Playwright E2E;
- deploy cloud;
- observabilidade avancada.

## Historico resumido de commits por etapa

### Documentacao inicial

- `13bc39c docs: add evaluation checklist`
- `d14fe8d docs: write final storytelling readme`
- `d5ebe09 docs: align final documentation with mvp scope`

### Backend

- `54ddc3a ci: add backend github actions workflow`
- `037d98e refactor(api): standardize errors and logging`
- `6b67e44 test: consolidate backend test coverage`

### IA

- `bef3692 feat(frontend): implement ai documentation flow`
- `a956738 test: cover questions endpoint integration flow`

### Testes

- `eb72f7b test(frontend): add automated tests`
- `6b67e44 test: consolidate backend test coverage`

### Frontend

- `49cd3de chore(frontend): add application structure`
- `a192414 feat(frontend): implement authentication screens`
- `2fe1c9c feat(frontend): implement project management flow`
- `bef3692 feat(frontend): implement ai documentation flow`
- `c06d1d0 fix(frontend): polish mvp user journey`

### CI

- `5074688 ci: validate backend and frontend`
- `6e01b34 fix(ci): diagnose and fix frontend npm ci lockfile issue`

### Docker

- `ce8afad chore: validate full docker compose execution`

### README final

- `d14fe8d docs: write final storytelling readme`

## Como validar a release

```bash
docker compose up --build
dotnet test src/backend/SpecPilot.sln
```

No frontend:

```powershell
Set-Location src/frontend/specpilot-web
npm ci
npm run build
npm test
```

## Resultado final

A versao MVP 1.0 do SpecPilot AI esta pronta para avaliacao academica. A entrega demonstra uso aplicado de IA Generativa com engenharia de software, testes automatizados, Docker, CI, documentacao versionada e desenvolvimento assistido por IA com supervisao humana.
