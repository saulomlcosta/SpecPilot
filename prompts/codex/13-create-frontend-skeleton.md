Crie o esqueleto do frontend do SpecPilot AI.

Contexto:
Estamos no Prompt 13.
O backend ja foi validado como pronto para servir o frontend.

Estado atual confirmado:
- Backend em .NET 8 funcional.
- PostgreSQL funcional via Docker Compose.
- Docker Compose validado.
- API disponivel em http://localhost:8080.
- Swagger disponivel em http://localhost:8080/swagger.
- Health check disponivel.
- FakeAiService como provider padrao.
- OpenAI provider opcional.
- Fluxo HTTP completo validado:
  - registro;
  - login;
  - GET /api/auth/me;
  - criacao de projeto;
  - geracao de perguntas;
  - GET /api/projects/{id}/questions;
  - resposta das perguntas;
  - geracao de documento;
  - GET /api/projects/{id}/document.
- Testes backend passando:
  - SpecPilot.UnitTests: 53/53
  - SpecPilot.IntegrationTests: 36/36
- GitHub Actions backend CI ja criado.
- Endpoint GET /api/projects/{id}/questions existe para permitir ao frontend obter questionIds reais.
- ProjectStatus avanca somente pelo fluxo controlado.
- Result Pattern + ProblemDetails estao implementados no backend.

Objetivo:
Criar a aplicacao frontend do SpecPilot AI sem implementar o fluxo funcional completo ainda.

Stack obrigatoria:
- React
- TypeScript
- Vite
- Tailwind CSS
- React Router
- TanStack Query
- React Hook Form
- Zod

Antes de comecar, leia obrigatoriamente:
- README.md
- AGENTS.md
- docs/02-mvp-scope.md
- docs/03-architecture.md
- docs/07-api-contracts.md
- docs/10-setup-guide.md
- docs/12-docker-strategy.md
- docs/13-development-best-practices.md
- docs/manual-backend-validation.md
- docs/development-log.md
- prompts/codex/12-consolidate-backend-tests.md
- prompts/codex/12a-add-github-actions-ci.md, se existir

Depois rode:
- git status
- git log --oneline -10

Tarefas:
1. Criar a aplicacao frontend em:
   - src/frontend/specpilot-web

2. Configurar:
   - React
   - TypeScript
   - Vite
   - Tailwind CSS
   - React Router
   - TanStack Query
   - React Hook Form
   - Zod

3. Criar estrutura inicial de pastas:

src/
  components/
  pages/
  services/
  hooks/
  schemas/
  routes/
  types/
  layouts/
  utils/

4. Criar paginas placeholder:
   - Login
   - Register
   - Projects
   - CreateProject
   - ProjectDetails
   - ProjectDocument
   - NotFound

5. Criar roteamento inicial:
   - /login
   - /register
   - /projects
   - /projects/new
   - /projects/:id
   - /projects/:id/document
   - fallback para NotFound

6. Criar layout basico:
   - layout publico para login/cadastro;
   - layout autenticado placeholder para paginas internas;
   - navegacao simples.

7. Criar cliente HTTP inicial para a API:
   - usar VITE_API_BASE_URL;
   - base URL padrao deve apontar para http://localhost:8080;
   - preparar suporte simples para token futuramente, mas sem implementar fluxo completo de autenticacao nesta etapa.

8. Criar tipos TypeScript iniciais alinhados ao contrato da API:
   - AuthResponse
   - UserResponse
   - ProjectResponse
   - RefinementQuestionResponse
   - ProjectDocumentResponse
   - ProblemDetails, se util para erros.

9. Criar schemas Zod iniciais para:
   - login;
   - cadastro;
   - criacao de projeto.

10. Criar componentes simples reutilizaveis:
   - Button
   - Input
   - Textarea
   - Card
   - FormError
   - LoadingState
   - EmptyState

11. Manter a UI simples, didatica e limpa.
12. Nao implementar ainda autenticacao completa.
13. Nao implementar ainda chamadas reais do fluxo de IA.
14. Nao implementar dashboard complexo.
15. Nao adicionar bibliotecas desnecessarias.
16. Nao criar funcionalidades fora do MVP.

Docker:
1. Criar Dockerfile do frontend em:
   - src/frontend/specpilot-web/Dockerfile

2. Atualizar docker-compose.yml para substituir o placeholder do frontend pelo frontend real.

3. Garantir que o frontend rode em:
   - http://localhost:3000

4. Garantir que a variavel de ambiente do frontend seja:
   - VITE_API_BASE_URL=http://localhost:8080

5. Manter compatibilidade com o backend e PostgreSQL ja existentes no Docker Compose.

6. O comando:
   docker compose up --build
   deve continuar subindo postgres, api e frontend.

Documentacao:
1. Atualizar README.md com instrucoes basicas do frontend, se necessario.
2. Atualizar docs/10-setup-guide.md com execucao do frontend, se necessario.
3. Atualizar docs/12-docker-strategy.md com o container do frontend real.
4. Atualizar docs/development-log.md adicionando entrada curta sobre o Prompt 13.

Rastreabilidade:
1. Salve este proprio prompt em:
   - prompts/codex/13-create-frontend-skeleton.md

2. Se o arquivo ja existir, atualize-o com esta versao.

Validacao:
1. Rodar instalacao/build/lint do frontend, se configurado.
2. Validar que a aplicacao frontend sobe localmente, se possivel.
3. Validar que docker-compose.yml continua coerente.
4. Rodar git status ao final.

Criterios de aceite:
- Aplicacao React criada em src/frontend/specpilot-web.
- TypeScript configurado.
- Tailwind configurado.
- Rotas placeholder criadas.
- Cliente HTTP inicial criado.
- Tipos e schemas iniciais criados.
- Componentes base criados.
- Dockerfile do frontend criado.
- docker-compose.yml atualizado para frontend real.
- README/docs atualizados se necessario.
- Nenhuma feature fora do MVP foi adicionada.
- Backend nao foi alterado desnecessariamente.

Nao implemente nesta etapa:
- login funcional;
- cadastro funcional;
- fluxo completo de projetos;
- geracao de perguntas;
- resposta de perguntas;
- geracao de documento;
- testes frontend;
- deploy;
- RAG;
- upload;
- PDF;
- chat livre;
- multiplos agentes.

Ao finalizar:
1. Rode validacoes aplicaveis.
2. Rode git status.
3. Liste arquivos alterados.
4. Faca commit usando Conventional Commits.

Mensagem de commit:
chore(frontend): add application structure
