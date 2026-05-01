Adicione suporte completo a Docker e Docker Compose para o projeto.

Antes de comecar, leia:

- README.md
- AGENTS.md
- docs/10-setup-guide.md
- docs/12-docker-strategy.md
- docs/13-development-best-practices.md

Objetivo:
Permitir que o avaliador consiga rodar o projeto futuramente com:

docker compose up --build

Servicos obrigatorios:
- PostgreSQL
- Backend API
- Frontend futuramente

Nesta etapa, como o frontend ainda nao existe, deixe o docker-compose preparado para frontend, mas nao quebre a execucao caso o frontend ainda nao esteja implementado.

Tarefas:
1. Criar Dockerfile para o backend em:
   - src/backend/SpecPilot.Api/Dockerfile
2. Atualizar docker-compose.yml na raiz.
3. Adicionar servico PostgreSQL.
4. Adicionar servico da API.
5. Configurar variaveis de ambiente da API.
6. Configurar connection string para PostgreSQL usando o hostname do container.
7. Garantir que a API fique acessivel em:
   - http://localhost:8080
8. Garantir que o Swagger fique acessivel em:
   - http://localhost:8080/swagger
9. Criar ou atualizar .env.example.
10. Configurar Ai Provider como Fake por padrao.
11. Atualizar README.md com instrucoes de execucao local usando Docker.
12. Atualizar docs/10-setup-guide.md se necessario.
13. Salvar este proprio prompt em:
    - prompts/codex/03-add-docker-support.md
14. Fazer commit ao final usando Conventional Commits.

Regras:
- Nao implemente frontend nesta etapa.
- Nao implemente endpoints funcionais.
- Nao implemente OpenAI.
- Nao adicione funcionalidades fora do MVP.
- A execucao com Docker nao deve depender de chave externa de IA.
- Segredos reais nao devem ser versionados.

Mensagem de commit:
chore: add docker compose setup
