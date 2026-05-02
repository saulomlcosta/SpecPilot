# Development Log

## Introducao

Este log registra a evolucao do desenvolvimento assistido por IA no projeto SpecPilot AI. O objetivo e manter um historico tecnico, didatico e rastreavel das etapas executadas, das decisoes tomadas e dos proximos passos previstos ao longo do projeto.

## Como este log deve ser usado

- cada entrada deve registrar uma etapa relevante do desenvolvimento
- o log nao deve conter segredos, tokens ou dados sensiveis
- o log deve apontar prompts, commits e decisoes importantes
- o log deve ajudar a retomar o trabalho apos troca de sessao, compactacao de contexto ou limite do Codex

## 2026-05-01 - Pausa por limite de uso do Codex

### Contexto

Durante a execucao do desenvolvimento assistido por IA, o Codex atingiu limite de uso antes da continuacao do Prompt 08.

### Ultimas etapas concluidas

- Prompt 00 - criacao da documentacao base.
- Prompt 01 - revisao da documentacao inicial.
- Prompt 02 - criacao do esqueleto backend.
- Prompt 03 - Docker/Docker Compose.
- Prompt 04 - autenticacao.
- Prompt 04a - Result Pattern + ProblemDetails no Auth.
- Prompt 05 - gerenciamento de projetos.
- Prompt 06 - abstracao de IA e FakeAiService.
- Prompt 07 - geracao de perguntas de refinamento.
- Prompt 07a - protecao das transicoes de status do projeto, se ja tiver sido aplicado.

### Proxima etapa planejada

- Prompt 08 - responder perguntas de refinamento.

### Decisoes importantes ja tomadas

- O projeto usa Result Pattern para falhas esperadas.
- O projeto usa ProblemDetails para respostas HTTP de erro.
- Exceptions devem representar falhas inesperadas, nao fluxo de negocio.
- O provider de IA padrao e FakeAiService.
- OpenAI sera opcional por variavel de ambiente.
- ProjectStatus nao deve ser alterado manualmente via PUT /api/projects/{id}.
- ProjectStatus deve avancar apenas pelos casos de uso do fluxo principal:
  - gerar perguntas;
  - responder perguntas;
  - gerar documento.

### Checklist antes de retomar

- Rodar git status.
- Rodar git log --oneline -10.
- Confirmar se o Prompt 07a foi aplicado e commitado.
- Confirmar se os testes atuais passam.
- Confirmar se nao ha alteracoes sem commit.
- Retomar pelo Prompt 08.

### Prompt de retomada recomendado

Recupere o contexto pelo repositorio antes de continuar.

Leia:
- README.md
- AGENTS.md
- docs/02-mvp-scope.md
- docs/06-data-model.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- docs/13-development-best-practices.md
- docs/development-log.md
- prompts/codex/

Depois rode:
- git status
- git log --oneline -10

Identifique a ultima etapa concluida.

Estamos prontos para continuar com o Prompt 08, desde que o Prompt 07a esteja aplicado e commitado.

Nao implemente nada fora do MVP.
Use os arquivos do repositorio como fonte da verdade.
Se houver conflito entre documentacao, codigo e prompts, pare e informe antes de alterar.

## 2026-05-01 - Prompt 11 concluido

- consolidado o uso de `ProblemDetails` para falhas esperadas e inesperadas
- migrado o tratamento global de excecoes para `IExceptionHandler`
- adicionados logs estruturados em fluxos principais e no provider OpenAI
- ampliados testes de integracao para validar respostas padronizadas de erro

## 2026-05-01 - Prompt 12 concluido

- consolidada a cobertura do fluxo principal do MVP com teste de jornada completa
- reforcados cenarios de `ProblemDetails` para validacao e acesso indevido
- fixado `Ai__Provider=Fake` na infraestrutura de testes de integracao
- ajustadas verificacoes de persistencia para `AsNoTracking()` e consultas assincronas

## 2026-05-01 - Prompt 12a concluido

- adicionada pipeline de GitHub Actions para restore, build e testes do backend
- fixado `Ai__Provider=Fake` no workflow para evitar chamadas reais a OpenAI
- documentada a decisao de nao subir PostgreSQL no CI porque os testes de integracao atuais usam banco em memoria

## 2026-05-02 - Validacao pre-frontend do backend

- validado que a API sobe em `http://localhost:8080` e o Swagger responde em `http://localhost:8080/swagger`
- identificado e corrigido o bloqueio de Docker em que o schema do PostgreSQL nao era criado no startup
- alinhadas as variaveis de OpenAI do `docker-compose.yml` com `Ai__OpenAi__ApiKey` e `Ai__OpenAi__Model`
- adicionado `GET /api/projects/{id}/questions` para o frontend recuperar `questionId` sem depender de acesso ao banco
- validado o fluxo principal completo em Docker com `Fake` como provider registrado em `AiInteractionLog`
- registrado roteiro manual para validacao futura do fluxo principal do MVP

## 2026-05-02 - Cobertura de integracao do endpoint de perguntas

- adicionada cobertura de integracao para `GET /api/projects/{id}/questions` com acesso negado a projeto de outro usuario
- ajustada a jornada principal automatizada para usar `GET /questions` e os `questionId` reais antes de responder o refinamento
- ampliada a validacao do fluxo completo para conferir provider `Fake` e os dois `AiInteractionLog` esperados

## 2026-05-02 - Prompt 13 concluido

- criado o esqueleto do frontend em React, TypeScript e Vite
- configurados Tailwind CSS, React Router, TanStack Query, React Hook Form e Zod
- adicionadas rotas, layouts, paginas placeholder e cliente HTTP inicial
- atualizado o Docker Compose para subir frontend, API e PostgreSQL juntos

## 2026-05-02 - Prompt 14 concluido

- implementadas telas funcionais de cadastro e login integradas ao backend
- adicionado gerenciamento de sessao com JWT em storage centralizado e `GET /api/auth/me`
- aplicadas rotas protegidas para as paginas internas e redirecionamentos de autenticacao
- implementado logout no layout autenticado com exibicao do usuario atual

## 2026-05-02 - Prompt 15 concluido

- implementado fluxo de projetos no frontend com listagem, criacao, detalhe, edicao e exclusao
- adicionada camada de servicos para `GET/POST/PUT/DELETE /api/projects` com tratamento de `ProblemDetails`
- mantida a regra de negocio do backend: `status` exibido, mas nunca enviado em create/update
- ajustadas as telas para estados de loading, erro amigavel, vazio e redirecionamentos apos mutacoes

## 2026-05-02 - Prompt 16 concluido

- implementado fluxo de IA no frontend com geracao de perguntas, resposta com `questionId` real e geracao de documento
- conectada a pagina de documento ao endpoint `GET /api/projects/{id}/document` com secoes estruturadas
- adicionados servicos e tipos para endpoints de refinamento e documento com tratamento amigavel de `ProblemDetails`
- mantida a orientacao de status do backend para habilitar apenas acoes compativeis em cada etapa

## 2026-05-02 - Prompt 16a concluido

- configurado frontend com Vitest, React Testing Library e ambiente `jsdom`
- adicionados testes automatizados enxutos para login, cadastro, criacao de projeto, detalhe por status e pagina de documento
- adicionados testes basicos para helpers de mensagem de erro e armazenamento de token
- atualizada documentacao com orientacoes de execucao de testes frontend

## 2026-05-02 - Prompt 16b concluido

- unificado o CI em `.github/workflows/ci.yml` com jobs separados para backend e frontend
- removido `backend-ci.yml` para evitar pipelines duplicadas
- configurado job frontend com `npm ci`, `npm run build` e `npm test` em modo nao interativo
- mantido `Ai__Provider=Fake` no backend CI para impedir chamadas reais a OpenAI

## 2026-05-02 - Correcao do lockfile do frontend para CI

- sincronizado `src/frontend/specpilot-web/package-lock.json` com `package.json` via `npm install`
- validado localmente o fluxo do CI frontend com `npm ci`, `npm run build` e `npm test`
- mantido o workflow usando `npm ci` no GitHub Actions, sem alterar escopo funcional

## 2026-05-02 - Reexecucao do CI apos sincronizacao do package-lock

- o erro original do GitHub Actions ocorreu no job Frontend durante `npm ci`, apontando `package-lock.json` fora de sincronia com `package.json`
- o log do CI indicou ausencia de `@emnapi/core@1.10.0`, `@emnapi/runtime@1.10.0` e `esbuild@0.28.0` no lockfile
- o commit `882f4f2 fix(frontend): sync npm lock file` ja registrou a correcao do lockfile do frontend
- no `HEAD` atual, `src/frontend/specpilot-web/package-lock.json` ja contem as dependencias reclamadas pelo workflow que falhou
- a hipotese mais provavel e que a execucao antiga do workflow rodou em commit anterior ao `882f4f2` ou em branch ainda nao atualizada
- o CI deve continuar usando `npm ci`, sem substituir por `npm install`, porque o comportamento correto em pipeline e validar precisamente o lockfile versionado
- sera criado apenas um commit vazio para disparar nova execucao do GitHub Actions no `HEAD` atual, sem alterar codigo funcional, dependencias ou workflow

## 2026-05-02 - Diagnostico definitivo da falha de npm ci no frontend

- o erro recorrente do GitHub Actions continuou apontando ausencia de `@emnapi/core@1.10.0`, `@emnapi/runtime@1.10.0` e `esbuild@0.28.0` durante `npm ci`
- a investigacao local confirmou que `package-lock.json` tinha referencias textuais a esses pacotes, mas nao continha estruturalmente `packages["node_modules/@emnapi/core"]`, `packages["node_modules/@emnapi/runtime"]` e `packages["node_modules/vitest/node_modules/esbuild"]`
- a raiz `packages[""]` do lockfile estava coerente com `package.json`, entao a causa raiz nao era dependencia declarada fora do lugar, e sim lockfile incompleto para o resolvedor usado no CI
- o lockfile foi regenerado no frontend com `npm` 10, alinhado ao ecossistema do runner com Node 20, passando a materializar corretamente os nos ausentes no mapa `packages`
- apos a regeneracao, `npm ci`, `npm run build` e `npm test` passaram localmente no frontend, sem alteracao funcional de codigo
- o workflow foi mantido com `npm ci` e recebeu apenas um passo temporario de diagnostico para a proxima execucao confirmar commit, diretorio e estrutura real do lockfile lido pelo runner
- a decisao permanece a mesma: nao substituir `npm ci` por `npm install` no CI, porque o objetivo da pipeline e validar exatamente o lockfile versionado

## 2026-05-02 - Prompt 17 concluido

- validada a subida completa do ambiente com `docker compose down -v` seguido de `docker compose up --build -d`
- confirmados os tres servicos esperados no Docker Compose: `postgres`, `api` e `frontend`, com PostgreSQL em estado `healthy`
- validados os endpoints principais da execucao local: frontend em `http://localhost:3000`, API em `http://localhost:8080`, Swagger em `http://localhost:8080/swagger` e health check em `http://localhost:8080/health`
- confirmada a configuracao padrao com `Ai__Provider=Fake` e sem necessidade de chave OpenAI para avaliacao local
- executado o fluxo principal do MVP contra a stack Docker: cadastro, login, criacao de projeto, geracao de perguntas, resposta das perguntas, geracao de documento e consulta do documento persistido
- confirmadas as transicoes de status `Draft`, `QuestionsGenerated`, `QuestionsAnswered` e `DocumentGenerated` no fluxo ponta a ponta da API usada pelo frontend
- confirmadas as secoes do documento gerado: visao geral, requisitos funcionais, requisitos nao funcionais, casos de uso e riscos
- reexecutados com sucesso `dotnet test src/backend/SpecPilot.sln`, `npm ci`, `npm run build`, `npm test` e `docker compose config`
- atualizada a documentacao de Docker para refletir que o frontend ja cobre o fluxo funcional atual do MVP

## 2026-05-02 - Prompt 18 concluido

- refinados textos e navegacao do frontend para deixar o fluxo principal mais claro para avaliadores
- centralizado o mapeamento amigavel de `ProjectStatus` e exibido o proximo passo conforme a etapa atual do projeto
- adicionados feedbacks de sucesso e estados disabled em acoes principais de autenticacao, projetos, refinamento e documento
- reforcado o tratamento de `ProblemDetails` no frontend para esconder detalhes tecnicos em erros internos
- validados `npm run build`, `npm test`, `dotnet test src/backend/SpecPilot.sln` e `docker compose config`

## 2026-05-02 - Prompt 19 concluido

- criado `docs/14-evaluation-checklist.md` com roteiro didatico para avaliadores validarem execucao, fluxo principal, uso de IA, testes, CI e limitacoes do MVP
- atualizado o README com link direto para o checklist de avaliacao
- registrado o proprio Prompt 19 em `prompts/codex/19-create-evaluation-checklist.md`

## 2026-05-02 - Prompt 20 concluido

- reescrito o `README.md` final em formato storytelling, com foco academico, didatico e profissional
- explicadas as duas dimensoes de uso de IA: dentro do produto e durante o desenvolvimento com Codex
- documentados prompts versionados, human-in-the-loop, skills/superpowers, Docker, testes, CI e principais decisoes arquiteturais

## 2026-05-02 - Prompt 21 concluido

- revisada a coerencia final entre README, documentacao principal, prompts e escopo real do MVP
- corrigidos documentos que ainda descreviam estado antigo do projeto como se backend e frontend nao estivessem implementados
- mantidas as funcionalidades fora do escopo apenas como limitacoes ou evolucoes futuras, sem promessas indevidas

## 2026-05-02 - Prompt 22 concluido

- criado `docs/15-release-notes.md` para registrar oficialmente a entrega da versao MVP 1.0
- atualizado o README com link para as release notes academicas
- salvo o Prompt 22 em `prompts/codex/22-create-release-notes.md`

## 2026-05-02 - Prompt 23 concluido

- executada a verificacao final de entrega com sucesso em backend, frontend, Docker Compose e documentacao principal
- validados `dotnet test src/backend/SpecPilot.sln`, `npm ci`, `npm run build`, `npm test`, `docker compose config`, `docker compose down -v`, `docker compose up --build -d` e `docker compose ps`
- confirmados os documentos finais do MVP: README storytelling, checklist de avaliacao, release notes, ADRs, prompts runtime, prompts Codex e development log
- confirmada a configuracao com `FakeAiService` como provider padrao, `OpenAI` opcional e ausencia de chamada direta do frontend para OpenAI
- verificado que os itens fora do escopo permanecem documentados apenas como limitacoes ou proximos passos

## 2026-05-02 - Prompt 24 concluido

- criada a rastreabilidade final para a publicacao da versao `v1.0.0` do MVP
- a tag `v1.0.0` aponta para o estado final apos o `final delivery check`
- a release do GitHub usa `docs/15-release-notes.md` como base documental
- nenhuma funcionalidade foi alterada nesta etapa, apenas o registro do processo de release

## 2026-05-02 - Correcao de CORS para autenticacao no frontend

- bug encontrado em teste manual pelo navegador ao chamar a API em `http://localhost:8080` a partir do frontend em `http://localhost:3000`
- causa raiz identificada: ausencia de configuracao explicita de CORS no backend ASP.NET Core para as origens locais do frontend
- correcao aplicada com a policy `SpecPilotFrontend`, configurada por `Cors__AllowedOrigins`, permitindo apenas `http://localhost:3000` e `http://127.0.0.1:3000`, com metodos do MVP e header `Authorization`
- validados o ciclo vermelho e verde com teste de integracao de preflight CORS, `dotnet test src/backend/SpecPilot.sln`, `npm ci`, `npm run build`, `npm test`, `docker compose config`, `docker compose up --build -d`, `OPTIONS /api/auth/login`, `POST /api/auth/register` e `POST /api/auth/login` com header `Origin`

## 2026-05-02 - Patch release v1.0.1

- criada a tag `v1.0.1` apos a correcao de CORS entre navegador, frontend e backend
- a tag `v1.0.0` continua apontando para o estado anterior da primeira release do MVP
- a versao `v1.0.1` passa a ser a versao recomendada para avaliacao
- nao houve nova funcionalidade; esta etapa registra apenas a correcao de integracao browser/frontend/backend
