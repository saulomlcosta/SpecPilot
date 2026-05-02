Adicione testes automatizados ao frontend do SpecPilot AI.

Contexto:
Estamos no Prompt 16a.
O Prompt 16 foi finalizado:
- fluxo de IA implementado no frontend;
- usuário gera perguntas pela UI;
- frontend busca perguntas com IDs reais via GET /api/projects/{id}/questions;
- usuário responde perguntas pela UI;
- frontend envia questionIds reais no PUT /questions/answers;
- usuário gera documento pela UI;
- usuário visualiza documento em /projects/:id/document;
- build do frontend passando;
- Docker Compose validado.

Objetivo:
Adicionar uma estratégia simples, efetiva e pequena de testes frontend, sem inflar o escopo do MVP.

Stack esperada:
- Vitest
- React Testing Library
- jsdom
- @testing-library/user-event
- @testing-library/jest-dom

Antes de começar, leia obrigatoriamente:
- README.md
- AGENTS.md
- docs/02-mvp-scope.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- docs/13-development-best-practices.md
- docs/development-log.md
- prompts/codex/14-implement-frontend-auth.md
- prompts/codex/15-implement-frontend-projects.md
- prompts/codex/16-implement-frontend-ai-flow.md

Depois rode:
- git status
- git log --oneline -10

Tarefas:
1. Configurar Vitest no frontend.
2. Configurar React Testing Library.
3. Configurar jsdom.
4. Configurar @testing-library/jest-dom.
5. Configurar script de teste no package.json, por exemplo:
   - npm test
   ou
   - npm run test

6. Criar setup de testes se necessário, por exemplo:
   - src/test/setup.ts
   - src/test/test-utils.tsx

7. Criar testes pequenos e de alto valor para componentes/formulários principais.

Cobertura mínima esperada:
1. Login:
   - renderiza campos de email e senha;
   - valida campos obrigatórios;
   - exibe erro amigável quando API retorna ProblemDetails.

2. Cadastro:
   - renderiza campos nome, email e senha;
   - valida campos obrigatórios;
   - valida email inválido.

3. Criação de projeto:
   - valida campos obrigatórios;
   - não inclui status no payload enviado à API, se for simples validar.

4. Detalhe de projeto:
   - renderiza dados principais do projeto;
   - exibe ação correta quando status é Draft;
   - exibe formulário de respostas quando status é QuestionsGenerated;
   - exibe ação de gerar documento quando status é QuestionsAnswered;
   - exibe link para documento quando status é DocumentGenerated.

5. Documento:
   - renderiza as seções:
     - Visão geral;
     - Requisitos funcionais;
     - Requisitos não funcionais;
     - Casos de uso;
     - Riscos.
   - exibe mensagem amigável quando documento não existe ou API retorna NotFound.

6. Serviços/helpers:
   - se houver parser centralizado de ProblemDetails, testar comportamento básico;
   - se houver helper de token, testar save/get/clear de forma simples.

Regras:
1. Não criar suíte grande demais.
2. Não testar detalhes frágeis de implementação.
3. Priorizar comportamento visível ao usuário.
4. Mockar chamadas HTTP quando necessário.
5. Não depender de backend real nesses testes.
6. Não depender de Docker.
7. Não fazer chamada real à OpenAI.
8. Não adicionar testes E2E com Playwright nesta etapa, a menos que seja muito simples e não aumente o escopo.
9. Se Playwright parecer grande, documente como melhoria futura.
10. Não alterar backend.
11. Não implementar novas funcionalidades.
12. Não alterar contratos da API.

Documentação:
1. Atualizar README.md com instruções de testes frontend, se necessário.
2. Atualizar docs/08-testing-strategy.md incluindo testes frontend com Vitest/React Testing Library.
3. Atualizar docs/13-development-best-practices.md citando testes frontend, se necessário.
4. Atualizar docs/development-log.md adicionando entrada curta sobre o Prompt 16a.

Rastreabilidade:
1. Salve este próprio prompt em:
   - prompts/codex/16a-add-frontend-tests.md
2. Se o arquivo já existir, atualize-o com esta versão.

Validação:
1. Rodar em src/frontend/specpilot-web:
   - npm run build
   - npm test
2. Validar:
   - docker compose config
3. Rodar git status ao final.

Critérios de aceite:
- Vitest configurado.
- React Testing Library configurado.
- Testes frontend passam.
- Build do frontend continua passando.
- Testes não dependem de backend real.
- Testes não fazem chamada real à OpenAI.
- Documentação de testes atualizada.
- Nenhuma funcionalidade fora do MVP foi adicionada.

Não implemente nesta etapa:
- Playwright completo, salvo se for trivial;
- CI frontend;
- deploy;
- RAG;
- upload;
- PDF;
- chat livre;
- múltiplos agentes;
- README storytelling final.

Ao finalizar:
1. Rode validações aplicáveis.
2. Rode git status.
3. Liste arquivos alterados.
4. Faça commit usando Conventional Commits.

Mensagem de commit:
test(frontend): add automated tests
