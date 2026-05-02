Faca uma revisao final do projeto contra o escopo do MVP.

Contexto:
Estamos no Prompt 21.
O README storytelling final foi concluido no commit:
d14fe8d docs: write final storytelling readme

O projeto SpecPilot AI ja possui:
- backend funcional e testado;
- frontend funcional e testado;
- fluxo principal do MVP implementado;
- Docker Compose validado;
- CI backend + frontend passando;
- checklist de avaliacao;
- README storytelling final;
- documentacao em docs/;
- ADRs;
- prompts runtime;
- prompts Codex;
- development log;
- FakeAiService como provider padrao;
- OpenAI provider opcional.

Objetivo:
Garantir que README, documentacao, prompts e codigo estejam coerentes com o escopo real do MVP, sem prometer funcionalidades inexistentes e sem incluir funcionalidades fora do escopo como se estivessem prontas.

Antes de comecar, leia obrigatoriamente:
- README.md
- AGENTS.md
- docs/02-mvp-scope.md
- docs/04-ai-usage.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- docs/10-setup-guide.md
- docs/12-docker-strategy.md
- docs/13-development-best-practices.md
- docs/14-evaluation-checklist.md
- docs/development-log.md
- docs/adr/
- prompts/runtime/
- prompts/codex/20-final-storytelling-readme.md

Depois rode:
- git status
- git log --oneline -10

Tarefas:
1. Revisar README.md.
2. Revisar docs/ principais.
3. Revisar docs/14-evaluation-checklist.md.
4. Revisar docs/development-log.md.
5. Revisar prompts/runtime e prompts/codex apenas para coerencia geral.
6. Verificar se o README nao diz que algo esta pendente quando ja foi implementado.
7. Verificar se o README nao diz que algo foi implementado quando nao existe.
8. Verificar se docs nao contradizem o estado atual do projeto.
9. Verificar se endpoints documentados batem com o contrato real.
10. Verificar se instrucoes de execucao estao coerentes.
11. Verificar se instrucoes de teste estao coerentes.
12. Verificar se Docker Compose esta documentado corretamente.
13. Verificar se GitHub Actions esta documentado corretamente.
14. Verificar se FakeAiService e OpenAI opcional estao explicados corretamente.
15. Verificar se o uso do Codex, prompts e human-in-the-loop esta explicado sem exageros.

Funcionalidades que devem aparecer apenas como fora do escopo, limitacoes ou proximos passos:
- RAG;
- upload de arquivos;
- exportacao PDF;
- chat livre;
- multiplos agentes;
- microservicos;
- Kafka;
- RabbitMQ;
- deploy cloud;
- colaboracao multiusuario;
- Playwright E2E, se nao foi implementado.

Escopo real implementado:
- autenticacao;
- projetos;
- geracao de perguntas;
- resposta de perguntas;
- geracao de documento;
- visualizacao de documento;
- backend .NET;
- frontend React;
- PostgreSQL;
- Docker Compose;
- testes backend;
- testes frontend;
- CI backend + frontend;
- prompts versionados;
- ADRs;
- development log;
- FakeAiService;
- OpenAI provider opcional.

Correcoes permitidas:
1. Corrigir README/docs se houver inconsistencias.
2. Corrigir links quebrados em documentacao.
3. Corrigir nomes de endpoints na documentacao.
4. Corrigir instrucoes de execucao/testes.
5. Corrigir qualquer mencao que aumente escopo indevidamente.
6. Corrigir qualquer trecho que pareca prometer recurso nao implementado.

Restricoes:
1. Nao implementar novas funcionalidades.
2. Nao alterar backend.
3. Nao alterar frontend funcional.
4. Nao alterar Docker, salvo documentacao incorreta.
5. Nao alterar workflow, salvo documentacao incorreta.
6. Nao aumentar escopo.
7. Nao reescrever README inteiro se apenas pequenos ajustes forem necessarios.

Validacao:
1. Validar links relativos principais, se possivel.
2. Validar Mermaid do README de forma sintatica ou visual, se possivel.
3. Rodar git status ao final.

Documentacao:
1. Atualizar docs/development-log.md com uma entrada curta sobre o Prompt 21.
2. Salvar este proprio prompt em:
   - prompts/codex/21-final-scope-review.md

Criterios de aceite:
- README esta coerente com o MVP real.
- Docs estao coerentes com o MVP real.
- Funcionalidades fora do escopo estao claramente marcadas como fora do escopo ou proximos passos.
- Nenhum documento promete recurso inexistente.
- Instrucoes de execucao e testes estao corretas.
- Links principais funcionam.
- Nenhum codigo funcional foi alterado.
- Prompt salvo em prompts/codex.
- development-log atualizado.

Ao finalizar:
1. Rode git status.
2. Liste arquivos alterados.
3. Faca commit usando Conventional Commits, se houver alteracoes.

Mensagem de commit, se houver alteracao:
docs: align final documentation with mvp scope
