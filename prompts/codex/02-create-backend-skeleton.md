Crie apenas o esqueleto do backend do SpecPilot AI com base na documentacao existente.

Antes de comecar, leia obrigatoriamente:

- README.md
- AGENTS.md
- docs/02-mvp-scope.md
- docs/03-architecture.md
- docs/06-data-model.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- docs/13-development-best-practices.md

Objetivo desta etapa:
Criar a estrutura inicial do backend sem implementar endpoints funcionais ainda.

Requisitos:
- .NET 8
- ASP.NET Core Web API
- Arquitetura em camadas:
  - SpecPilot.Api
  - SpecPilot.Application
  - SpecPilot.Domain
  - SpecPilot.Infrastructure
- Entity Framework Core
- PostgreSQL
- MediatR
- FluentValidation
- JWT preparado para etapas futuras
- xUnit para testes

Tarefas:
1. Crie a solution .NET dentro de src/backend.
2. Crie os projetos:
   - SpecPilot.Api
   - SpecPilot.Application
   - SpecPilot.Domain
   - SpecPilot.Infrastructure
3. Configure as referencias entre projetos corretamente:
   - Api referencia Application e Infrastructure
   - Application referencia Domain
   - Infrastructure referencia Application e Domain
4. Crie os projetos de teste:
   - tests/backend/SpecPilot.UnitTests
   - tests/backend/SpecPilot.IntegrationTests
5. Crie as entidades principais no dominio:
   - User
   - Project
   - RefinementQuestion
   - ProjectDocument
   - AiInteractionLog
6. Crie o enum ProjectStatus com:
   - Draft
   - QuestionsGenerated
   - QuestionsAnswered
   - DocumentGenerated
7. Crie o DbContext no projeto Infrastructure.
8. Configure Entity Framework Core com PostgreSQL.
9. Configure injecao de dependencia inicial.
10. Configure Swagger no projeto Api.
11. Configure health check simples para a API.
12. Nao implemente endpoints de autenticacao ainda.
13. Nao implemente endpoints de projeto ainda.
14. Nao integre OpenAI ainda.
15. Nao implemente frontend.
16. Salve este proprio prompt em:
    - prompts/codex/02-create-backend-skeleton.md
17. Faca commit ao final usando Conventional Commits.

Regras de qualidade:
- Nao colocar logica de negocio em controllers.
- Nao criar abstracoes desnecessarias.
- Nao adicionar funcionalidades fora do MVP.
- Usar nomes claros.
- Manter a solucao simples e testavel.

Mensagem de commit:
chore: add backend solution structure
