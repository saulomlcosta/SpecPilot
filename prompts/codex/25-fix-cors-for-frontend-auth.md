# Prompt 25 - Corrigir CORS para autenticacao do frontend

Recupere o contexto pelo repositório antes de continuar.

Contexto:
O SpecPilot AI já foi finalizado como MVP, mas durante um teste manual como avaliador foi identificado um problema real no fluxo pelo navegador.

Problema:
Quando o frontend em http://localhost:3000 chama o backend em http://localhost:8080, o navegador bloqueia a requisição por CORS.

Objetivo:
Corrigir a configuração de CORS no backend ASP.NET Core para permitir que o frontend local acesse a API em ambiente de desenvolvimento/Docker Compose, sem abrir permissões excessivas.

Antes de alterar, leia:
- README.md
- AGENTS.md
- docs/10-setup-guide.md
- docs/12-docker-strategy.md
- docs/development-log.md
- src/backend/SpecPilot.Api/Program.cs
- src/backend/SpecPilot.Api/appsettings.json
- src/backend/SpecPilot.Api/appsettings.Development.json, se existir
- docker-compose.yml
- .env.example
- src/frontend/specpilot-web/src/services/httpClient.ts

Depois rode:
- git status
- git log --oneline -10

Tarefas:
1. Adicionar configuração de CORS no backend ASP.NET Core.

2. Permitir explicitamente as origens:
   - http://localhost:3000
   - http://127.0.0.1:3000

3. Não usar AllowAnyOrigin de forma ampla.

4. Não permitir credentials se não for necessário.

5. Garantir que o header Authorization seja permitido.

6. Garantir que os métodos usados pela aplicação funcionem:
   - GET
   - POST
   - PUT
   - DELETE
   - OPTIONS

7. Criar uma policy clara, por exemplo:
   - SpecPilotFrontend

8. Garantir que app.UseCors(...) esteja na ordem correta do pipeline ASP.NET Core.

9. Garantir que a policy seja aplicada antes dos endpoints/controllers.

10. Preferir configuração por variável de ambiente/appsettings para ficar didático.

Sugestão de configuração:
- Cors__AllowedOrigins=http://localhost:3000;http://127.0.0.1:3000

11. Atualizar docker-compose.yml se usar configuração por ambiente:
   - Cors__AllowedOrigins=http://localhost:3000;http://127.0.0.1:3000

12. Atualizar .env.example se usar essa variável.

13. Não alterar frontend, salvo se encontrar URL incorreta.

14. Não alterar contrato da API.

15. Não implementar novas funcionalidades.

Testes:
1. Se viável, adicionar um teste de integração simples para CORS.

O teste deve validar que uma requisição com Origin http://localhost:3000 recebe header:
- Access-Control-Allow-Origin: http://localhost:3000

Pode ser uma requisição OPTIONS/preflight ou uma requisição GET simples com Origin.

2. Se o teste de CORS ficar grande demais ou frágil, documentar a validação manual em docs/development-log.md.

Validação manual obrigatória:
1. Rodar:
   - docker compose down -v --remove-orphans
   - docker compose up --build -d

2. Abrir:
   - http://localhost:3000/register

3. Criar conta com dados válidos:
   - name: Usuário Teste
   - email: teste-[timestamp]@example.com
   - password: 12345678

4. Confirmar no Network:
   - OPTIONS /api/auth/register, se houver preflight
   - POST /api/auth/register
   - sem erro de CORS

5. Fazer logout.

6. Abrir:
   - http://localhost:3000/login

7. Fazer login.

8. Confirmar no Network:
   - OPTIONS /api/auth/login, se houver preflight
   - POST /api/auth/login
   - sem erro de CORS

9. Confirmar redirecionamento para:
   - /projects

Validação técnica:
1. Rodar:
   - dotnet test src/backend/SpecPilot.sln

2. Em src/frontend/specpilot-web, rodar:
   - npm ci
   - npm run build
   - npm test

3. Na raiz, rodar:
   - docker compose config

Documentação:
1. Atualizar docs/development-log.md com uma entrada curta informando:
   - bug encontrado no teste manual pelo navegador;
   - causa raiz: CORS entre frontend localhost:3000 e API localhost:8080;
   - correção aplicada;
   - validações executadas.

2. Atualizar docs/10-setup-guide.md se houver nova variável de ambiente ou instrução relevante.

3. Atualizar docs/12-docker-strategy.md se a configuração de CORS impactar Docker Compose.

4. Não reescrever README inteiro.

5. Não alterar release notes, a menos que seja estritamente necessário.

Rastreabilidade:
1. Criar o arquivo:
   - prompts/codex/25-fix-cors-for-frontend-auth.md

2. Salvar este próprio prompt nesse arquivo.

Critérios de aceite:
- Frontend em http://localhost:3000 consegue chamar API em http://localhost:8080.
- Cadastro funciona pela UI.
- Login funciona pela UI.
- Não há erro de CORS no navegador.
- CORS não fica aberto com AllowAnyOrigin amplo.
- Authorization header é permitido.
- Métodos necessários são permitidos.
- Testes backend passam.
- Testes frontend passam.
- docker compose config passa.
- Docker Compose continua subindo postgres, api e frontend.
- Documentação e development log foram atualizados.
- Prompt salvo em prompts/codex.
- Nenhuma funcionalidade fora do MVP foi adicionada.

Ao finalizar:
1. Rode git status.
2. Liste arquivos alterados.
3. Faça commit usando Conventional Commits.

Mensagem de commit:
fix(api): allow frontend origin with cors
