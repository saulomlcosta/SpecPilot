Execute a verificacao final de entrega do SpecPilot AI.

Contexto:
Estamos no Prompt 23, a etapa final do projeto.
O projeto ja possui:
- backend funcional;
- frontend funcional;
- fluxo principal completo;
- testes backend;
- testes frontend;
- Docker Compose validado;
- CI backend + frontend passando;
- README storytelling final;
- checklist de avaliacao;
- release notes;
- documentacao, ADRs, prompts e development log versionados.

Objetivo:
Garantir que o projeto esta pronto para ser clonado, executado, testado e avaliado.

Antes de comecar, leia obrigatoriamente:
- README.md
- AGENTS.md
- docs/02-mvp-scope.md
- docs/10-setup-guide.md
- docs/12-docker-strategy.md
- docs/14-evaluation-checklist.md
- docs/15-release-notes.md
- docs/development-log.md
- .github/workflows/ci.yml
- prompts/codex/20-final-storytelling-readme.md
- prompts/codex/21-final-scope-review.md
- prompts/codex/22-create-release-notes.md

Depois rode:
- git status
- git log --oneline -15

Tarefas de verificacao:
1. Confirmar que README.md esta coerente com o estado real do projeto.
2. Confirmar que docs/14-evaluation-checklist.md existe.
3. Confirmar que docs/15-release-notes.md existe.
4. Confirmar que docs/development-log.md esta atualizado.
5. Confirmar que prompts/runtime existem.
6. Confirmar que prompts/codex existem e registram o processo.
7. Confirmar que docs/adr possui ADRs relevantes.
8. Confirmar que .env.example existe e nao contem segredo real.
9. Confirmar que docker-compose.yml existe e esta coerente.
10. Confirmar que .github/workflows/ci.yml existe.
11. Confirmar que CI valida backend e frontend.
12. Confirmar que nao ha mencao indevida a recurso fora do MVP como se estivesse implementado.
13. Confirmar que RAG, upload, PDF, chat livre, multiplos agentes, deploy cloud e Playwright E2E aparecem apenas como fora do escopo ou proximos passos.
14. Confirmar que FakeAiService e explicado como provider padrao.
15. Confirmar que OpenAI e opcional.
16. Confirmar que frontend nao chama OpenAI diretamente.

Validacoes tecnicas obrigatorias:
1. Rodar:
   - dotnet test src/backend/SpecPilot.sln

2. Em src/frontend/specpilot-web, rodar:
   - npm ci
   - npm run build
   - npm test

3. Na raiz, rodar:
   - docker compose config

4. Se o Docker daemon estiver disponivel, rodar:
   - docker compose down -v
   - docker compose up --build -d
   - docker compose ps

5. Se Docker subir, validar:
   - http://localhost:3000
   - http://localhost:8080
   - http://localhost:8080/swagger
   - http://localhost:8080/health

6. Se o ambiente nao permitir Docker, registrar a limitacao em docs/development-log.md sem tratar como erro do projeto.

Validacao de links:
1. Validar links relativos principais no README.
2. Validar links para:
   - docs/14-evaluation-checklist.md
   - docs/15-release-notes.md
   - docs/development-log.md
   - docs/adr/
   - prompts/runtime/
   - prompts/codex/

Validacao de seguranca:
1. Verificar se nao ha API key real versionada.
2. Verificar se nao ha token JWT real versionado.
3. Verificar se .env nao foi versionado, se existir localmente.
4. Verificar se apenas .env.example esta versionado.
5. Verificar se README orienta a nao versionar segredos.

Correcoes permitidas:
1. Corrigir links quebrados.
2. Corrigir instrucoes de execucao se estiverem erradas.
3. Corrigir documentacao incoerente.
4. Corrigir pequenos problemas de texto.
5. Corrigir somente problemas bloqueantes de entrega.

Restricoes:
1. Nao implementar novas funcionalidades.
2. Nao alterar escopo.
3. Nao alterar backend ou frontend funcional salvo bug bloqueante.
4. Nao adicionar RAG.
5. Nao adicionar upload.
6. Nao adicionar PDF.
7. Nao adicionar chat livre.
8. Nao adicionar multiplos agentes.
9. Nao adicionar deploy.
10. Nao reescrever README inteiro.

Documentacao:
1. Atualizar docs/development-log.md com uma entrada final sobre o Prompt 23.
2. Salvar este proprio prompt em:
   - prompts/codex/23-final-delivery-check.md

Criterios de aceite:
- Backend tests passam.
- Frontend build passa.
- Frontend tests passam.
- docker compose config passa.
- Docker Compose sobe, se ambiente permitir.
- README esta coerente.
- Checklist existe.
- Release notes existem.
- Prompts estao versionados.
- ADRs existem.
- CI esta configurado.
- Segredos nao estao versionados.
- Escopo esta correto.
- git status final esta limpo.

Ao finalizar:
1. Rode git status.
2. Liste arquivos alterados.
3. Faca commit usando Conventional Commits se houver alteracoes.

Mensagem de commit, se houver alteracao:
chore: final delivery check
