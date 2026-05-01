Implemente a fatia de gerenciamento de projetos do SpecPilot AI.

Antes de comecar, leia:

- AGENTS.md
- docs/02-mvp-scope.md
- docs/06-data-model.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- docs/13-development-best-practices.md

Objetivo:
Permitir que um usuario autenticado crie e gerencie seus proprios projetos.

Endpoints:
- POST /api/projects
- GET /api/projects
- GET /api/projects/{id}
- PUT /api/projects/{id}
- DELETE /api/projects/{id}

Campos do projeto:
- Name
- InitialDescription
- Goal
- TargetAudience
- Status
- CreatedAt
- UpdatedAt

Regras:
1. Apenas usuarios autenticados podem acessar projetos.
2. Um usuario so pode visualizar, atualizar ou excluir seus proprios projetos.
3. Todo projeto novo deve comecar com status Draft.
4. Nao permitir nome vazio.
5. Nao permitir descricao inicial vazia.
6. Nao permitir objetivo vazio.
7. Nao permitir publico-alvo vazio.
8. Usar MediatR para casos de uso.
9. Usar FluentValidation.
10. Nao colocar logica de negocio nos controllers.
11. Criar testes unitarios dos handlers e validators.
12. Criar testes de integracao dos endpoints.
13. Atualizar docs/07-api-contracts.md se necessario.
14. Salvar este proprio prompt em:
    - prompts/codex/05-implement-projects.md
15. Fazer commit ao final usando Conventional Commits.

Criterios de aceite:
- Usuario autenticado consegue criar projeto.
- Usuario autenticado consegue listar apenas seus projetos.
- Usuario autenticado consegue buscar um projeto seu por ID.
- Usuario nao consegue acessar projeto de outro usuario.
- Usuario consegue atualizar projeto proprio.
- Usuario consegue excluir projeto proprio.
- Testes passam.

Nao implemente:
- IA
- Perguntas
- Documento
- Frontend
- Compartilhamento de projetos
- Colaboracao multiusuario

Mensagem de commit:
feat(projects): implement project management

Antes de implementar Projects, considere que o projeto agora usa Result Pattern + ProblemDetails, conforme:

- AGENTS.md
- docs/13-development-best-practices.md
- docs/adr/0009-use-result-pattern-and-problemdetails.md

Durante esta etapa:
- Handlers devem retornar Result ou Result<T>.
- Falhas esperadas devem usar Result.Failure.
- Nao use exceptions para fluxo esperado.
- Controllers/endpoints devem converter Result para respostas HTTP com ProblemDetails.
- Mantenha Domain/Application desacoplados de ASP.NET Core.

Aplique isso em todos os casos de Projects:
- projeto nao encontrado -> NotFound
- projeto de outro usuario -> Forbidden ou NotFound, conforme estrategia documentada
- entrada invalida -> Validation
- conflito de regra, se houver -> Conflict
