Implemente as telas e o fluxo de autenticacao no frontend do SpecPilot AI.

Contexto:
Estamos no Prompt 14.
O Prompt 13 ja foi finalizado:
- Frontend criado em src/frontend/specpilot-web.
- React + TypeScript + Vite configurados.
- Tailwind CSS configurado.
- React Router configurado.
- TanStack Query configurado.
- React Hook Form e Zod instalados/configurados.
- Paginas placeholder criadas.
- Dockerfile do frontend criado.
- docker-compose.yml agora sobe postgres, api e frontend.
- Frontend respondeu 200 em http://localhost:3000.

Backend validado:
- API em http://localhost:8080.
- Swagger em http://localhost:8080/swagger.
- FakeAiService como padrao.
- Auth endpoints disponiveis:
  - POST /api/auth/register
  - POST /api/auth/login
  - GET /api/auth/me
- Backend usa JWT.
- Erros usam ProblemDetails.
- Result Pattern + ProblemDetails estao implementados no backend.

Objetivo:
Implementar cadastro, login, logout, leitura do usuario autenticado e protecao de rotas no frontend.

Antes de comecar, leia obrigatoriamente:
- README.md
- AGENTS.md
- docs/02-mvp-scope.md
- docs/07-api-contracts.md
- docs/10-setup-guide.md
- docs/12-docker-strategy.md
- docs/13-development-best-practices.md
- docs/development-log.md
- prompts/codex/13-create-frontend-skeleton.md

Depois rode:
- git status
- git log --oneline -10

Tarefas:
1. Implementar tela de cadastro em:
   - /register

2. Implementar tela de login em:
   - /login

3. Integrar com os endpoints:
   - POST /api/auth/register
   - POST /api/auth/login
   - GET /api/auth/me

4. Usar React Hook Form + Zod para validar:
   - cadastro:
     - name obrigatorio;
     - email obrigatorio e valido;
     - password obrigatorio com minimo compativel com backend.
   - login:
     - email obrigatorio e valido;
     - password obrigatorio.

5. Usar TanStack Query para mutations/queries:
   - register;
   - login;
   - me.

6. Criar ou ajustar servico de API para:
   - usar import.meta.env.VITE_API_BASE_URL;
   - fallback para http://localhost:8080;
   - enviar Authorization: Bearer <token> quando houver token;
   - tratar ProblemDetails de forma amigavel.

7. Implementar armazenamento simples do token para o MVP.
   - Pode usar localStorage.
   - Centralizar acesso ao token em um service/helper.
   - Nao espalhar localStorage diretamente por varias telas.

8. Criar AuthContext ou hook equivalente para:
   - armazenar usuario autenticado;
   - indicar loading inicial;
   - login;
   - register;
   - logout;
   - refresh/me.

9. Implementar protecao de rotas:
   - usuario nao autenticado deve ser redirecionado para /login ao tentar acessar rotas internas;
   - usuario autenticado nao precisa ficar preso em login/register;
   - rotas internas protegidas:
     - /projects
     - /projects/new
     - /projects/:id
     - /projects/:id/document

10. Apos login ou cadastro bem-sucedido:
    - salvar token;
    - carregar usuario autenticado se necessario;
    - redirecionar para /projects.

11. Implementar logout:
    - remover token;
    - limpar usuario;
    - redirecionar para /login.

12. Ajustar layout autenticado:
    - exibir nome ou email do usuario autenticado;
    - exibir botao de logout;
    - manter navegacao simples.

13. Exibir estados de UI:
    - loading;
    - erro de validacao;
    - erro vindo da API em ProblemDetails;
    - sucesso/redirecionamento.

14. Manter UI simples, didatica e limpa.
15. Nao implementar ainda fluxo de projetos alem do necessario para redirecionamento.
16. Nao implementar geracao de perguntas.
17. Nao implementar resposta de perguntas.
18. Nao implementar geracao de documento.
19. Nao implementar testes frontend nesta etapa.
20. Nao adicionar funcionalidades fora do MVP.

Docker/ambiente:
1. Garantir que o frontend continue usando:
   - VITE_API_BASE_URL=http://localhost:8080
2. Garantir que docker-compose.yml continue coerente.
3. Nao alterar backend se nao for estritamente necessario.

Documentacao:
1. Atualizar README.md somente se houver instrucao real de autenticacao/execucao que precise ser ajustada.
2. Atualizar docs/10-setup-guide.md se necessario.
3. Atualizar docs/development-log.md adicionando entrada curta sobre o Prompt 14.

Rastreabilidade:
1. Salve este proprio prompt em:
   - prompts/codex/14-implement-frontend-auth.md

2. Se o arquivo ja existir, atualize-o com esta versao.

Validacao:
1. Rodar:
   - npm run build
   em src/frontend/specpilot-web.

2. Se possivel, rodar a aplicacao localmente ou via Docker Compose e validar manualmente:
   - abrir http://localhost:3000;
   - acessar /register;
   - cadastrar usuario;
   - ser redirecionado para /projects;
   - fazer logout;
   - fazer login;
   - acessar /projects autenticado;
   - tentar acessar /projects sem token e ser redirecionado para /login.

3. Validar:
   - docker compose config
   - docker compose up --build -d, se viavel.

4. Rodar git status ao final.

Criterios de aceite:
- Cadastro funcional pelo frontend.
- Login funcional pelo frontend.
- Token JWT salvo de forma centralizada.
- GET /api/auth/me usado para recuperar usuario autenticado.
- Rotas internas protegidas.
- Logout funcional.
- Erros de API exibidos de forma amigavel.
- Build do frontend passa.
- Docker Compose continua coerente.
- Nenhuma funcionalidade fora do MVP foi adicionada.
- Backend nao foi alterado desnecessariamente.

Nao implemente nesta etapa:
- CRUD completo de projetos;
- geracao de perguntas;
- resposta de perguntas;
- geracao de documento;
- testes frontend;
- dashboard complexo;
- RAG;
- upload;
- PDF;
- chat livre;
- multiplos agentes;
- deploy.

Ao finalizar:
1. Rode validacoes aplicaveis.
2. Rode git status.
3. Liste arquivos alterados.
4. Faca commit usando Conventional Commits.

Mensagem de commit:
feat(frontend): implement authentication screens
