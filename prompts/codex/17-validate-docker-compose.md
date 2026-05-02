Valide a execução completa do SpecPilot AI com Docker Compose.

Contexto:
Estamos no Prompt 17.
O projeto já possui:
- backend .NET 8 funcional;
- PostgreSQL via Docker Compose;
- frontend React + TypeScript + Vite funcional;
- fluxo principal do MVP implementado no frontend;
- testes backend consolidados;
- testes frontend adicionados;
- GitHub Actions validando backend e frontend;
- CI verde após correção do package-lock do frontend;
- FakeAiService como provider padrão;
- OpenAI provider opcional.

Objetivo:
Garantir que o projeto completo possa ser executado por um avaliador com:

docker compose up --build

E que o fluxo principal do MVP funcione ponta a ponta via frontend.

Antes de começar, leia obrigatoriamente:
- README.md
- AGENTS.md
- docs/02-mvp-scope.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- docs/10-setup-guide.md
- docs/12-docker-strategy.md
- docs/13-development-best-practices.md
- docs/manual-backend-validation.md
- docs/development-log.md
- prompts/codex/16-implement-frontend-ai-flow.md
- prompts/codex/16a-add-frontend-tests.md
- prompts/codex/16b-update-github-actions-full-ci.md

Depois rode:
- git status
- git log --oneline -10

Tarefas principais:
1. Validar docker-compose.yml.
2. Validar Dockerfile do backend.
3. Validar Dockerfile do frontend.
4. Validar .env.example.
5. Garantir que o projeto sobe com:
   - docker compose down -v
   - docker compose up --build -d

6. Confirmar que os containers sobem corretamente:
   - postgres
   - api
   - frontend

7. Confirmar que o PostgreSQL fica saudável.
8. Confirmar que a API sobe em:
   - http://localhost:8080

9. Confirmar que o Swagger abre em:
   - http://localhost:8080/swagger

10. Confirmar que o health check responde em:
   - http://localhost:8080/health

11. Confirmar que o frontend abre em:
   - http://localhost:3000

12. Confirmar que o frontend chama corretamente a API em:
   - http://localhost:8080

13. Confirmar que o backend roda com:
   - Ai__Provider=Fake

14. Confirmar que nenhuma chave OpenAI é necessária para o fluxo padrão.
15. Confirmar que nenhuma chamada real à OpenAI é feita no fluxo padrão.

Validação funcional via frontend:
1. Abrir http://localhost:3000.
2. Criar um usuário pelo frontend.
3. Fazer login pelo frontend.
4. Acessar /projects.
5. Criar um projeto.
6. Abrir detalhes do projeto.
7. Gerar perguntas de refinamento.
8. Buscar/exibir perguntas geradas.
9. Responder todas as perguntas.
10. Gerar documento.
11. Abrir página do documento.
12. Confirmar que o documento exibe:
    - Visão geral;
    - Requisitos funcionais;
    - Requisitos não funcionais;
    - Casos de uso;
    - Riscos.

13. Confirmar que as transições de status aparecem corretamente na UI:
    - Draft;
    - QuestionsGenerated;
    - QuestionsAnswered;
    - DocumentGenerated.

14. Confirmar que logout funciona.
15. Confirmar que rota protegida redireciona usuário não autenticado para login.

Validação técnica:
1. Rode:
   - dotnet test src/backend/SpecPilot.sln

2. Rode em src/frontend/specpilot-web:
   - npm ci
   - npm run build
   - npm test

3. Rode:
   - docker compose config

4. Rode:
   - docker compose ps

5. Se possível, registre evidências no development log:
   - containers que subiram;
   - URLs validadas;
   - fluxo funcional validado;
   - testes executados.

Correções permitidas:
1. Corrigir apenas problemas necessários para a execução completa com Docker.
2. Corrigir inconsistência de variável de ambiente.
3. Corrigir erro de CORS se o frontend não conseguir chamar API.
4. Corrigir documentação de setup se estiver divergente.
5. Corrigir problema pequeno de Dockerfile ou docker-compose.

Restrições:
1. Não implementar novas funcionalidades.
2. Não alterar escopo do MVP.
3. Não implementar RAG.
4. Não implementar upload.
5. Não implementar PDF.
6. Não implementar chat livre.
7. Não implementar múltiplos agentes.
8. Não implementar deploy.
9. Não publicar imagem Docker.
10. Não reescrever README storytelling final nesta etapa.
11. Não alterar backend ou frontend se não houver bug real de execução.

Documentação:
1. Atualizar docs/10-setup-guide.md se houver divergência real.
2. Atualizar docs/12-docker-strategy.md se houver ajuste em Docker.
3. Atualizar docs/development-log.md adicionando entrada sobre o Prompt 17.
4. Atualizar README.md somente se instrução de execução estiver incorreta.
5. Não reescrever o README completo nesta etapa.

Rastreabilidade:
1. Salve este próprio prompt em:
   - prompts/codex/17-validate-docker-compose.md

2. Se o arquivo já existir, atualize-o com esta versão.

Critérios de aceite:
- docker compose up --build -d sobe postgres, api e frontend.
- http://localhost:3000 responde.
- http://localhost:8080 responde.
- http://localhost:8080/swagger abre.
- http://localhost:8080/health responde.
- Fluxo principal funciona via frontend.
- FakeAiService é usado por padrão.
- OpenAI não é obrigatória para avaliação.
- Backend tests passam.
- Frontend build passa.
- Frontend tests passam.
- Docker Compose config é válido.
- Documentação de execução está coerente.
- Nenhuma funcionalidade fora do MVP foi adicionada.

Ao finalizar:
1. Rode validações aplicáveis.
2. Rode git status.
3. Liste arquivos alterados.
4. Faça commit somente se houver alteração.

Mensagem de commit, se houver alteração:
chore: validate full docker compose execution
