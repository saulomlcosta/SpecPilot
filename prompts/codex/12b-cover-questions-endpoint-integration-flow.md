Antes de iniciar o Prompt 13, feche as lacunas de teste identificadas para o novo endpoint GET /api/projects/{id}/questions.

Contexto:
O backend foi validado para servir o frontend, e o endpoint GET /api/projects/{id}/questions foi adicionado para permitir que a UI obtenha os questionIds reais antes de responder perguntas.

A suite atual passa, mas faltam duas coberturas importantes:
1. Teste de integracao garantindo que outro usuario nao acessa perguntas de projeto alheio via GET /api/projects/{id}/questions.
2. Teste de jornada completa automatizada usando GET /questions como passo intermediario para obter questionIds reais antes de responder perguntas.

Antes de alterar:
1. Leia AGENTS.md.
2. Leia docs/07-api-contracts.md.
3. Leia docs/08-testing-strategy.md.
4. Leia docs/development-log.md.
5. Leia os testes atuais em:
   - tests/backend/SpecPilot.IntegrationTests/Projects/GenerateRefinementQuestionsEndpointTests.cs
   - tests/backend/SpecPilot.IntegrationTests, especialmente o teste de jornada principal, se existir.
6. Rode git status.

Tarefas:
1. Adicionar teste de integracao para:
   - usuario A cria projeto;
   - usuario A gera perguntas;
   - usuario B tenta GET /api/projects/{id}/questions;
   - resposta deve ser 404 NotFound;
   - se o projeto usa ProblemDetails, validar o corpo com code=projects.not_found ou equivalente documentado.

2. Adicionar ou ajustar teste de jornada completa do MVP para usar explicitamente:
   - register/login;
   - create project;
   - POST /api/projects/{id}/generate-questions;
   - GET /api/projects/{id}/questions;
   - usar os questionIds retornados pelo GET /questions;
   - PUT /api/projects/{id}/questions/answers;
   - POST /api/projects/{id}/generate-document;
   - GET /api/projects/{id}/document.

3. Garantir que esse teste valide as transicoes:
   - Draft;
   - QuestionsGenerated;
   - QuestionsAnswered;
   - DocumentGenerated.

4. Garantir que o teste valide, quando possivel:
   - 2 registros em AiInteractionLog no fluxo completo;
   - provider Fake;
   - tipos GenerateRefinementQuestions e GenerateProjectDocument.

5. Manter FakeAiService como provider nos testes.
6. Garantir que nenhuma chamada real a OpenAI seja feita.
7. Usar AsNoTracking e chamadas async nas consultas ao banco.
8. Nao alterar comportamento da API se nao for necessario.
9. Nao implementar frontend.
10. Nao aumentar escopo.

Documentacao:
1. Atualizar docs/08-testing-strategy.md apenas se necessario.
2. Atualizar docs/development-log.md com uma entrada curta informando que a cobertura de integracao do GET /questions e da jornada com questionIds reais foi adicionada.
3. Salvar este prompt em:
   - prompts/codex/12b-cover-questions-endpoint-integration-flow.md

Validacao:
1. Rode:
   - dotnet test src/backend/SpecPilot.sln
2. Rode:
   - git status

Ao finalizar, faca commit usando Conventional Commits.

Mensagem de commit:
test: cover questions endpoint integration flow
