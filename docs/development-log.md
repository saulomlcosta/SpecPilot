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
