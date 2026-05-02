Melhore tratamento de erros, validações, logs e respostas padronizadas da API.

Contexto:
Estamos no Prompt 11 do SpecPilot AI.
O projeto já possui:
- Auth
- Projects
- FakeAiService
- geração de perguntas
- resposta das perguntas
- geração de documento
- OpenAI provider opcional
- Result Pattern + ProblemDetails iniciado no Prompt 04a
- ProjectStatus protegido contra alteração manual
- endpoints de documento padronizados como:
  - POST /api/projects/{id}/generate-document
  - GET /api/projects/{id}/document

Objetivo:
Consolidar a estratégia de tratamento de erros da API, padronizar respostas HTTP, revisar uso do Result Pattern, implementar Global Exception Handler para exceções inesperadas e melhorar logs estruturados.

Antes de começar, leia obrigatoriamente:
- README.md
- AGENTS.md
- docs/02-mvp-scope.md
- docs/04-ai-usage.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- docs/09-security.md
- docs/13-development-best-practices.md
- docs/adr/0009-use-result-pattern-and-problemdetails.md
- prompts/codex/04a-add-result-pattern-and-problemdetails-to-auth.md
- prompts/codex/10-integrate-openai-provider.md

Depois rode:
- git status
- git log --oneline -10

Tarefas principais:
1. Revisar se os handlers usam Result ou Result<T> para falhas esperadas.
2. Garantir que exceptions não sejam usadas para fluxo esperado de negócio.
3. Manter exceptions apenas para falhas inesperadas.
4. Centralizar o mapeamento de Result para respostas HTTP.
5. Usar ProblemDetails para respostas HTTP de erro.
6. Implementar ou consolidar Global Exception Handler para exceções inesperadas.
7. Garantir que o Global Exception Handler não substitui o Result Pattern.
8. Garantir que Domain e Application não dependem de ASP.NET Core.
9. Garantir que ProblemDetails fique restrito à camada Api.
10. Revisar controllers/endpoints para remover lógica de negócio.
11. Revisar handlers para evitar duplicação desnecessária.
12. Revisar validações com FluentValidation.

Mapeamento esperado entre ErrorType e HTTP:
- Validation -> 400 Bad Request
- Unauthorized -> 401 Unauthorized
- Forbidden -> 403 Forbidden
- NotFound -> 404 Not Found
- Conflict -> 409 Conflict
- Failure -> 500 Internal Server Error

Regras sobre erros esperados:
- email duplicado deve retornar Conflict.
- credenciais inválidas devem retornar Unauthorized.
- projeto inexistente deve retornar NotFound.
- projeto de outro usuário deve retornar NotFound para evitar vazamento de informação.
- status inválido de fluxo deve retornar Conflict.
- documento ausente deve retornar NotFound.
- tentativa de gerar documento antes de responder perguntas deve retornar Conflict.
- tentativa de gerar perguntas fora de Draft deve retornar Conflict.
- payload inválido deve retornar Validation.

Global Exception Handler:
1. Implementar usando abordagem adequada ao .NET 8, preferencialmente IExceptionHandler + ProblemDetails.
2. Retornar ProblemDetails genérico para falhas inesperadas.
3. Não expor stack trace em resposta HTTP.
4. Não expor dados sensíveis.
5. Logar a exceção com contexto suficiente para diagnóstico.
6. Manter mensagem pública segura para o cliente.

Logs:
1. Adicionar ou revisar logs estruturados básicos nos pontos relevantes:
   - geração de perguntas;
   - resposta das perguntas;
   - geração de documento;
   - chamada ao provider OpenAI;
   - falhas inesperadas.
2. Não logar:
   - senha;
   - token JWT;
   - API key;
   - segredos;
   - Authorization header completo.
3. Quando logar usuário/projeto, preferir IDs e não dados sensíveis.

OpenAI Provider:
1. Revisar tratamento de:
   - timeout;
   - falha HTTP não 2xx;
   - JSON inválido;
   - resposta vazia;
   - API key ausente quando Ai__Provider=OpenAI.
2. Essas falhas devem ser tratadas de forma controlada.
3. Testes automatizados não devem fazer chamada real à OpenAI.
4. FakeAiService deve continuar sendo padrão.

Testes:
1. Atualizar ou criar testes para respostas ProblemDetails.
2. Cobrir pelo menos:
   - Unauthorized sem token.
   - NotFound para projeto de outro usuário.
   - Conflict para status inválido de fluxo.
   - NotFound para documento ausente.
   - Conflict para email duplicado, se ainda não estiver coberto.
   - Unauthorized para credenciais inválidas.
3. Garantir que testes existentes continuem passando.
4. Rodar:
   - dotnet test

Documentação:
1. Atualizar docs/09-security.md se necessário.
2. Atualizar docs/13-development-best-practices.md com a consolidação da estratégia.
3. Atualizar docs/08-testing-strategy.md se novos testes forem adicionados.
4. Atualizar docs/07-api-contracts.md se o formato de erro ficar mais claro.
5. Atualizar README.md somente se houver seção sobre erros, testes ou qualidade que precise refletir essa mudança.
6. Atualizar docs/development-log.md adicionando uma entrada curta sobre o Prompt 11.

Rastreabilidade:
1. Salve este próprio prompt em:
   - prompts/codex/11-improve-api-errors-and-logging.md

2. Se já existir esse arquivo, atualize-o com esta versão consolidada.

Critérios de aceite:
- Result Pattern é usado para falhas esperadas.
- ProblemDetails é usado para respostas HTTP de erro.
- Global Exception Handler trata exceções inesperadas.
- Domain e Application continuam desacoplados de ASP.NET Core.
- Exceptions não são usadas como fluxo esperado de negócio.
- Logs não expõem dados sensíveis.
- OpenAI provider possui tratamento controlado de falhas.
- Testes relevantes passam.
- Documentação e development log são atualizados.
- O escopo do MVP não aumenta.

Não implemente:
- frontend
- RAG
- upload
- PDF
- múltiplos agentes
- chat livre
- deploy
- GitHub Actions nesta etapa

Ao finalizar:
1. Rode dotnet test.
2. Rode git status.
3. Liste os arquivos alterados.
4. Faça commit usando Conventional Commits.

Mensagem de commit:
refactor(api): standardize errors and logging
