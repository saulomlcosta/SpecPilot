Implemente o fluxo de projetos no frontend do SpecPilot AI.

Contexto:
Estamos no Prompt 15.
O Prompt 14 foi finalizado:
- cadastro funcional no frontend;
- login funcional no frontend;
- logout funcional;
- token JWT salvo de forma centralizada;
- GET /api/auth/me usado para recuperar usuário autenticado;
- rotas internas protegidas;
- layout autenticado exibindo usuário e logout;
- frontend buildando corretamente.

Backend validado:
- API em http://localhost:8080.
- Swagger em http://localhost:8080/swagger.
- Docker Compose sobe postgres, api e frontend.
- Auth endpoints:
  - POST /api/auth/register
  - POST /api/auth/login
  - GET /api/auth/me
- Projects endpoints:
  - POST /api/projects
  - GET /api/projects
  - GET /api/projects/{id}
  - PUT /api/projects/{id}
  - DELETE /api/projects/{id}
- Backend usa Result Pattern + ProblemDetails.
- Usuário não pode acessar projeto de outro usuário.
- ProjectStatus não pode ser alterado manualmente via update.
- O update comum de projeto deve alterar apenas:
  - name
  - initialDescription
  - goal
  - targetAudience

Objetivo:
Implementar no frontend o gerenciamento de projetos do MVP.

Antes de começar, leia obrigatoriamente:
- README.md
- AGENTS.md
- docs/02-mvp-scope.md
- docs/07-api-contracts.md
- docs/10-setup-guide.md
- docs/12-docker-strategy.md
- docs/13-development-best-practices.md
- docs/development-log.md
- prompts/codex/13-create-frontend-skeleton.md
- prompts/codex/14-implement-frontend-auth.md

Depois rode:
- git status
- git log --oneline -10

Tarefas:
1. Implementar página de listagem de projetos:
   - rota: /projects
   - consumir GET /api/projects
   - exibir cards ou lista simples de projetos
   - exibir nome, objetivo, público-alvo e status
   - exibir estado vazio quando não houver projetos
   - exibir loading
   - exibir erro amigável se a API falhar

2. Implementar página de criação de projeto:
   - rota: /projects/new
   - consumir POST /api/projects
   - campos:
     - name
     - initialDescription
     - goal
     - targetAudience
   - validar com React Hook Form + Zod
   - após criar, redirecionar para /projects/{id}
   - não enviar status no payload

3. Implementar página de detalhes do projeto:
   - rota: /projects/:id
   - consumir GET /api/projects/{id}
   - exibir:
     - name
     - initialDescription
     - goal
     - targetAudience
     - status
     - createdAt
     - updatedAt, se existir
   - exibir ações disponíveis conforme status, mas sem implementar ainda o fluxo de IA:
     - se Draft, mostrar texto indicando que perguntas poderão ser geradas na próxima etapa
     - se QuestionsGenerated, mostrar texto indicando que perguntas poderão ser respondidas na próxima etapa
     - se QuestionsAnswered, mostrar texto indicando que documento poderá ser gerado na próxima etapa
     - se DocumentGenerated, mostrar link para /projects/{id}/document

4. Implementar edição simples de projeto:
   - pode ser na própria página de detalhes ou em um bloco/formulário simples
   - consumir PUT /api/projects/{id}
   - permitir editar apenas:
     - name
     - initialDescription
     - goal
     - targetAudience
   - não permitir editar status
   - não enviar status no payload
   - após salvar, atualizar os dados da tela

5. Implementar exclusão de projeto:
   - consumir DELETE /api/projects/{id}
   - pedir confirmação simples antes de excluir
   - após excluir, redirecionar para /projects
   - manter UI simples

6. Usar TanStack Query:
   - query para lista de projetos
   - query para detalhe de projeto
   - mutation para criar
   - mutation para atualizar
   - mutation para excluir
   - invalidar queries relevantes após mutations

7. Usar serviço de API existente:
   - reaproveitar cliente HTTP criado no Prompt 14
   - manter Authorization Bearer funcionando
   - tratar ProblemDetails de forma amigável

8. Criar/ajustar types TypeScript para:
   - ProjectResponse
   - CreateProjectRequest
   - UpdateProjectRequest
   - ProjectStatus

9. Criar/ajustar schemas Zod para:
   - createProjectSchema
   - updateProjectSchema

10. Manter UI simples, didática e consistente com o Prompt 14.

11. Garantir que rotas internas continuam protegidas.

12. Não implementar ainda:
   - geração de perguntas;
   - GET /api/projects/{id}/questions;
   - resposta de perguntas;
   - geração de documento;
   - visualização real do documento;
   - testes frontend;
   - dashboard complexo.

Contrato e regras importantes:
1. O frontend não deve permitir edição manual de ProjectStatus.
2. O frontend não deve enviar status em POST ou PUT de projeto.
3. O frontend deve respeitar que ProjectStatus é controlado pelo backend.
4. Se a API retornar NotFound em projeto, exibir mensagem amigável e permitir voltar para /projects.
5. Se a API retornar ProblemDetails, exibir title/detail ou mensagem equivalente.
6. Não criar funcionalidades fora do MVP.

Docker/ambiente:
1. Garantir que o frontend continua usando:
   - VITE_API_BASE_URL=http://localhost:8080
2. Garantir que docker-compose.yml não seja quebrado.
3. Não alterar backend se não for estritamente necessário.

Documentação:
1. Atualizar README.md somente se houver instrução real de execução que precise ser ajustada.
2. Atualizar docs/10-setup-guide.md se necessário.
3. Atualizar docs/development-log.md adicionando entrada curta sobre o Prompt 15.
4. Não reescrever README storytelling nesta etapa.

Rastreabilidade:
1. Salve este próprio prompt em:
   - prompts/codex/15-implement-frontend-projects.md
2. Se o arquivo já existir, atualize-o com esta versão.

Validação:
1. Rodar:
   - npm run build
   em src/frontend/specpilot-web.

2. Se possível, validar manualmente via Docker Compose:
   - docker compose up --build -d
   - abrir http://localhost:3000
   - login/cadastro
   - acessar /projects
   - criar projeto
   - abrir detalhe do projeto
   - editar projeto
   - confirmar que status não é editável
   - excluir projeto
   - confirmar retorno para lista

3. Validar:
   - docker compose config

4. Rodar git status ao final.

Critérios de aceite:
- Usuário autenticado consegue listar projetos.
- Usuário autenticado consegue criar projeto.
- Usuário autenticado consegue visualizar detalhe do projeto.
- Usuário autenticado consegue editar campos permitidos.
- Usuário autenticado consegue excluir projeto.
- Status é exibido, mas não editável.
- Frontend não envia status no create/update.
- Erros ProblemDetails são exibidos de forma amigável.
- Build do frontend passa.
- Docker Compose continua coerente.
- Nenhuma funcionalidade fora do MVP foi adicionada.
- Backend não foi alterado desnecessariamente.

Não implemente nesta etapa:
- geração de perguntas;
- resposta de perguntas;
- geração de documento;
- testes frontend;
- RAG;
- upload;
- PDF;
- chat livre;
- múltiplos agentes;
- deploy.

Ao finalizar:
1. Rode validações aplicáveis.
2. Rode git status.
3. Liste arquivos alterados.
4. Faça commit usando Conventional Commits.

Mensagem de commit:
feat(frontend): implement project management flow
