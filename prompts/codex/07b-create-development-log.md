Crie um arquivo de log de desenvolvimento para registrar a evolucao do projeto de forma didatica e rastreavel.

Contexto:
Estamos desenvolvendo o SpecPilot AI com apoio do Codex, seguindo prompts versionados em prompts/codex/.
O projeto e uma entrega academica de pos-graduacao em IA Generativa.
O objetivo do development log e registrar marcos importantes do desenvolvimento, decisoes tomadas, prompts executados, commits e proximos passos.

Tarefas:
1. Crie o arquivo:
   - docs/development-log.md

2. O arquivo deve ser escrito em portugues, com tom tecnico, objetivo e didatico.

3. Adicione uma introducao explicando que o log registra a evolucao do desenvolvimento assistido por IA.

4. Inclua uma secao chamada:
   ## Como este log deve ser usado

Explique que:
- cada entrada deve registrar uma etapa relevante;
- o log nao deve conter segredos, tokens ou dados sensiveis;
- o log deve apontar prompts, commits e decisoes;
- o log deve ajudar a retomar o trabalho apos troca de sessao, compactacao de contexto ou limite do Codex.

5. Adicione uma entrada inicial com o status atual:

Titulo da entrada:
## 2026-05-01 - Pausa por limite de uso do Codex

Conteudo da entrada:

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

6. Atualize README.md adicionando um link discreto para:
   - docs/development-log.md

Sugestao de secao:
- Desenvolvimento assistido por IA
ou
- Documentacao complementar

7. Atualize docs/11-codex-development-process.md citando que docs/development-log.md e usado para registrar marcos e retomadas de contexto.

8. Salve este proprio prompt em:
   - prompts/codex/07b-create-development-log.md

9. Nao implemente nenhuma funcionalidade de aplicacao.
10. Nao altere backend, frontend, testes ou Docker.
11. Rode git status ao final.
12. Faca commit usando Conventional Commits.

Mensagem de commit:
docs: add development log
