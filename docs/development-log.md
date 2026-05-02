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
