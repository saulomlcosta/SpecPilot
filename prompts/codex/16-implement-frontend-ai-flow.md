Implemente o fluxo de IA no frontend do SpecPilot AI.

Contexto:
Estamos no Prompt 16.
O Prompt 15 foi finalizado:
- listagem de projetos funcional;
- criação de projeto funcional;
- detalhe de projeto funcional;
- edição de campos permitidos funcional;
- exclusão de projeto funcional;
- frontend não envia status em create/update;
- TanStack Query aplicado no fluxo de projetos;
- build do frontend passando;
- Docker Compose validado.

Backend validado:
- API em http://localhost:8080.
- Swagger em http://localhost:8080/swagger.
- FakeAiService como provider padrão.
- OpenAI provider opcional.
- Result Pattern + ProblemDetails implementados.
- ProjectStatus controlado apenas pelo fluxo.
- Backend não depende de frontend.
- Testes backend passando.

Fluxo backend validado:
- register/login;
- create project;
- POST /api/projects/{id}/generate-questions;
- GET /api/projects/{id}/questions;
- PUT /api/projects/{id}/questions/answers;
- POST /api/projects/{id}/generate-document;
- GET /api/projects/{id}/document.

Objetivo:
Implementar na interface o fluxo principal de IA do MVP:

1. Gerar perguntas de refinamento.
2. Buscar perguntas geradas com seus IDs.
3. Responder perguntas.
4. Gerar documento técnico inicial.
5. Visualizar documento gerado.

Antes de começar, leia obrigatoriamente:
- README.md
- AGENTS.md
- docs/02-mvp-scope.md
- docs/04-ai-usage.md
- docs/07-api-contracts.md
- docs/10-setup-guide.md
- docs/12-docker-strategy.md
- docs/13-development-best-practices.md
- docs/manual-backend-validation.md
- docs/development-log.md
- prompts/codex/13-create-frontend-skeleton.md
- prompts/codex/14-implement-frontend-auth.md
- prompts/codex/15-implement-frontend-projects.md

Depois rode:
- git status
- git log --oneline -10

Endpoints a consumir:
- POST /api/projects/{id}/generate-questions
- GET /api/projects/{id}/questions
- PUT /api/projects/{id}/questions/answers
- POST /api/projects/{id}/generate-document
- GET /api/projects/{id}/document

Tarefas:

1. Criar ou ajustar serviços frontend para o fluxo de IA:
   - generateQuestions(projectId)
   - getQuestions(projectId)
   - answerQuestions(projectId, answers)
   - generateDocument(projectId)
   - getDocument(projectId)

2. Criar ou ajustar types TypeScript para:
   - RefinementQuestionResponse
   - GenerateQuestionsResponse
   - AnswerQuestionsRequest
   - AnswerQuestionItem
   - ProjectDocumentResponse
   - FunctionalRequirement
   - NonFunctionalRequirement
   - UseCase
   - Risk

3. Garantir que o frontend usa os questionIds reais retornados por:
   - GET /api/projects/{id}/questions

4. Atualizar ProjectDetailsPage para exibir ações conforme status:

   Draft:
   - mostrar botão "Gerar perguntas de refinamento";
   - ao clicar, chamar POST /generate-questions;
   - após sucesso, invalidar queries do projeto e perguntas;
   - exibir perguntas ou orientar o usuário para a etapa de respostas.

   QuestionsGenerated:
   - buscar perguntas com GET /questions;
   - exibir formulário para responder perguntas;
   - usar questionId real em cada resposta;
   - exigir resposta para todas as perguntas;
   - enviar PUT /questions/answers;
   - após sucesso, invalidar projeto e perguntas.

   QuestionsAnswered:
   - mostrar botão "Gerar documento";
   - ao clicar, chamar POST /generate-document;
   - após sucesso, invalidar projeto e documento;
   - redirecionar ou exibir link para /projects/{id}/document.

   DocumentGenerated:
   - mostrar link/botão para visualizar documento em /projects/{id}/document.

5. Implementar formulário de respostas:
   - pode ficar em ProjectDetailsPage ou em componente separado.
   - usar React Hook Form + Zod, se fizer sentido.
   - garantir que todas as perguntas tenham resposta.
   - exibir loading, erro e sucesso.
   - manter UI simples e didática.

6. Implementar ProjectDocumentPage:
   - rota: /projects/:id/document
   - consumir GET /api/projects/{id}/document
   - exibir seções:
     - Visão geral
     - Requisitos funcionais
     - Requisitos não funcionais
     - Casos de uso
     - Riscos
   - usar componentes simples e legíveis.
   - tratar loading.
   - tratar NotFound de forma amigável.
   - oferecer link para voltar ao projeto.

7. Atualizar comportamento da listagem/detalhe, se necessário:
   - status deve refletir o backend após cada ação;
   - não permitir ações incompatíveis com o status;
   - desabilitar botões durante mutations;
   - exibir mensagens claras.

8. Tratar ProblemDetails:
   - mostrar mensagem amigável com base em title/detail/code.
   - não exibir stack trace.
   - manter tratamento centralizado se já existir.

9. Garantir que o fluxo funciona com FakeAiService por padrão.
10. Não fazer chamada direta à OpenAI pelo frontend.
11. Não colocar API key no frontend.
12. Não criar chat livre.
13. Não criar RAG.
14. Não criar upload.
15. Não criar PDF.
16. Não criar múltiplos agentes.
17. Não criar dashboard complexo.

Regras de frontend:
1. Manter a UI simples, didática e consistente com os prompts anteriores.
2. Não duplicar lógica desnecessária.
3. Reaproveitar componentes base existentes.
4. Manter chamadas de API em services.
5. Manter tipos em types.
6. Manter validação de formulário em schemas quando fizer sentido.
7. Não alterar backend, salvo correção mínima absolutamente necessária.
8. Não alterar contrato da API sem necessidade.

Docker/ambiente:
1. Garantir que o frontend continua usando:
   - VITE_API_BASE_URL=http://localhost:8080
2. Garantir que docker-compose.yml continua funcionando.
3. Não quebrar API, Postgres ou Docker Compose.

Documentação:
1. Atualizar README.md somente se alguma instrução real de execução precisar ajuste.
2. Atualizar docs/10-setup-guide.md se necessário.
3. Atualizar docs/development-log.md adicionando entrada curta sobre o Prompt 16.
4. Não reescrever README storytelling nesta etapa.

Rastreabilidade:
1. Salve este próprio prompt em:
   - prompts/codex/16-implement-frontend-ai-flow.md
2. Se o arquivo já existir, atualize-o com esta versão.

Validação:
1. Rodar:
   - npm run build
   em src/frontend/specpilot-web.

2. Se possível, validar manualmente via Docker Compose:
   - docker compose up --build -d
   - abrir http://localhost:3000
   - cadastrar ou logar;
   - criar projeto;
   - abrir detalhe;
   - gerar perguntas;
   - responder perguntas;
   - gerar documento;
   - abrir /projects/{id}/document;
   - confirmar que o documento é exibido.

3. Validar:
   - docker compose config

4. Se alterações acidentais ocorrerem no backend, rodar:
   - dotnet test src/backend/SpecPilot.sln

5. Rodar git status ao final.

Critérios de aceite:
- Usuário autenticado consegue gerar perguntas pela UI.
- Frontend busca perguntas com IDs reais via GET /questions.
- Usuário consegue responder todas as perguntas pela UI.
- Frontend envia questionIds reais no PUT /questions/answers.
- Usuário consegue gerar documento pela UI.
- Usuário consegue visualizar documento em /projects/:id/document.
- Status do projeto guia as ações disponíveis.
- Ações incompatíveis com o status não ficam disponíveis.
- Erros ProblemDetails aparecem de forma amigável.
- Build do frontend passa.
- Docker Compose continua coerente.
- Nenhuma funcionalidade fora do MVP foi adicionada.
- Backend não foi alterado desnecessariamente.

Não implemente nesta etapa:
- testes frontend;
- CI frontend;
- RAG;
- upload;
- PDF;
- chat livre;
- múltiplos agentes;
- deploy;
- README storytelling final.

Ao finalizar:
1. Rode validações aplicáveis.
2. Rode git status.
3. Liste arquivos alterados.
4. Faça commit usando Conventional Commits.

Mensagem de commit:
feat(frontend): implement ai documentation flow
