Consolide a cobertura de testes do backend do SpecPilot AI.

Contexto:
Estamos no Prompt 12.
O projeto ja possui:
- autenticacao;
- gerenciamento de projetos;
- Result Pattern + ProblemDetails;
- protecao das transicoes de ProjectStatus;
- FakeAiService;
- geracao de perguntas de refinamento;
- resposta das perguntas;
- geracao de documento com FakeAiService;
- OpenAI provider opcional;
- tratamento padronizado de erros e logs consolidado no Prompt 11.

Objetivo:
Garantir que o fluxo principal do MVP esteja coberto por testes unitarios e de integracao, sem depender de chamadas reais a OpenAI.

Fluxo principal a cobrir:
1. Registrar usuario.
2. Fazer login.
3. Criar projeto.
4. Gerar perguntas com FakeAiService.
5. Responder perguntas.
6. Gerar documento com FakeAiService.
7. Buscar documento gerado.

Antes de comecar, leia obrigatoriamente:
- README.md
- AGENTS.md
- docs/02-mvp-scope.md
- docs/04-ai-usage.md
- docs/06-data-model.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- docs/09-security.md
- docs/13-development-best-practices.md
- docs/development-log.md
- docs/adr/0009-use-result-pattern-and-problemdetails.md
- prompts/codex/11-improve-api-errors-and-logging.md

Depois rode:
- git status
- git log --oneline -10

Tarefas:
1. Revisar os testes unitarios existentes.
2. Revisar os testes de integracao existentes.
3. Identificar lacunas de cobertura no fluxo principal do MVP.
4. Adicionar testes faltantes sem aumentar o escopo funcional.
5. Garantir que os testes usem FakeAiService.
6. Garantir que nenhum teste faca chamada real a OpenAI.
7. Garantir que os testes possam ser executados com:
   - dotnet test

Testes unitarios esperados:
1. Auth:
   - registro com sucesso;
   - email duplicado;
   - login com sucesso;
   - credenciais invalidas.
2. Projects:
   - criacao de projeto;
   - update sem alterar status;
   - protecao contra alteracao manual de ProjectStatus;
   - projeto nao encontrado;
   - projeto de outro usuario tratado como NotFound.
3. Questions:
   - gerar perguntas somente quando status for Draft;
   - nao gerar perguntas fora de Draft;
   - responder perguntas somente quando status for QuestionsGenerated;
   - nao aceitar respostas incompletas.
4. Documents:
   - gerar documento somente quando status for QuestionsAnswered;
   - nao gerar documento antes das respostas;
   - buscar documento existente;
   - retornar NotFound quando documento nao existir.
5. AI:
   - FakeAiService retorna respostas previsiveis;
   - OpenAI provider nao e usado nos testes;
   - selecao de provider por configuracao, se ja houver teste para isso.

Testes de integracao esperados:
1. Auth:
   - POST /api/auth/register;
   - POST /api/auth/login;
   - GET /api/auth/me com token valido;
   - GET /api/auth/me sem token.
2. Projects:
   - POST /api/projects;
   - GET /api/projects;
   - GET /api/projects/{id};
   - PUT /api/projects/{id};
   - DELETE /api/projects/{id};
   - usuario nao acessa projeto de outro usuario;
   - update comum nao altera status.
3. Questions:
   - POST /api/projects/{id}/generate-questions;
   - GET /api/projects/{id}/questions, se existir;
   - PUT /api/projects/{id}/questions/answers;
   - status muda de Draft para QuestionsGenerated;
   - status muda de QuestionsGenerated para QuestionsAnswered;
   - AiInteractionLog e criado na geracao de perguntas.
4. Documents:
   - POST /api/projects/{id}/generate-document;
   - GET /api/projects/{id}/document;
   - status muda de QuestionsAnswered para DocumentGenerated;
   - AiInteractionLog e criado na geracao de documento;
   - usuario nao acessa documento de projeto de outro usuario;
   - GET /document retorna NotFound quando documento ainda nao existe.
5. Error responses:
   - validar pelo menos alguns cenarios ProblemDetails:
     - Unauthorized sem token;
     - NotFound para projeto de outro usuario;
     - Conflict para status invalido de fluxo;
     - BadRequest/Validation para payload invalido.

Regras de qualidade dos testes:
1. Nao depender de ordem de execucao.
2. Nao compartilhar estado entre testes de forma fragil.
3. Usar nomes claros no padrao Should_...
4. Evitar sleeps/timeouts desnecessarios.
5. Evitar chamadas externas.
6. Preferir asserts de comportamento observavel.
7. Quando acessar banco em teste de integracao, preferir AsNoTracking e chamadas async.
8. Helpers de teste devem reduzir duplicacao sem esconder comportamento importante.
9. Nao testar detalhes irrelevantes de implementacao.
10. Nao enfraquecer regras de negocio apenas para facilitar testes.

Banco e infraestrutura de testes:
1. Se ja estiver usando Testcontainers, manter Testcontainers.
2. Se estiver usando PostgreSQL de teste, garantir isolamento adequado.
3. Se estiver usando WebApplicationFactory, manter configuracao clara.
4. Garantir que Ai__Provider seja Fake no ambiente de testes.
5. Garantir que OpenAI nao seja chamado em testes.

Result Pattern + ProblemDetails:
1. Falhas esperadas devem continuar retornando Result.Failure.
2. Testes devem validar status HTTP e, quando relevante, ProblemDetails.
3. Nao usar exceptions para cenarios esperados de negocio.

Documentacao:
1. Atualizar README.md com instrucoes reais para rodar testes, se necessario.
2. Atualizar docs/08-testing-strategy.md com a cobertura atual.
3. Atualizar docs/13-development-best-practices.md se necessario.
4. Atualizar docs/development-log.md adicionando uma entrada curta sobre o Prompt 12.

Rastreabilidade:
1. Salve este proprio prompt em:
   - prompts/codex/12-consolidate-backend-tests.md

2. Se o arquivo ja existir, atualize-o com esta versao.

Criterios de aceite:
- dotnet test passa.
- O fluxo principal do MVP esta coberto por testes unitarios e/ou integracao.
- Testes nao dependem de OpenAI real.
- FakeAiService e usado em testes.
- Cenarios principais de autorizacao estao cobertos.
- Cenarios principais de transicao de status estao cobertos.
- Alguns erros ProblemDetails estao cobertos.
- Documentacao e development log sao atualizados.
- O escopo do MVP nao aumenta.

Nao implemente:
- frontend;
- GitHub Actions;
- RAG;
- upload;
- PDF;
- multiplos agentes;
- chat livre;
- deploy.

Ao finalizar:
1. Rode:
   - dotnet test
2. Rode:
   - git status
3. Liste os arquivos alterados.
4. Faca commit usando Conventional Commits.

Mensagem de commit:
test: consolidate backend test coverage
