Atualize o GitHub Actions para validar backend e frontend.

Contexto:
Estamos no Prompt 16b.
O Prompt 16a já foi finalizado:
- testes frontend adicionados;
- Vitest configurado;
- React Testing Library configurado;
- jsdom configurado;
- npm test passando;
- npm run build passando.

O projeto já possui:
- backend .NET 8 com testes unitários e integração;
- frontend React + TypeScript + Vite;
- Docker Compose validado;
- GitHub Actions backend CI criado anteriormente;
- fluxo principal do MVP funcional;
- FakeAiService como provider padrão;
- OpenAI provider opcional.

Objetivo:
Atualizar o CI para validar automaticamente backend e frontend em push e pull request, sem deploy e sem uso de serviços externos de IA.

Antes de começar, leia obrigatoriamente:
- README.md
- AGENTS.md
- docs/08-testing-strategy.md
- docs/13-development-best-practices.md
- docs/development-log.md
- prompts/codex/12a-add-github-actions-ci.md
- prompts/codex/16a-add-frontend-tests.md
- .github/workflows/backend-ci.yml, se existir

Depois rode:
- git status
- git log --oneline -10

Tarefas:
1. Atualizar ou criar workflow em:
   - .github/workflows/ci.yml

2. Se já existir .github/workflows/backend-ci.yml, escolha uma abordagem simples:
   - substituir por ci.yml único para backend + frontend;
   - ou manter backend-ci.yml e criar frontend-ci.yml.

Preferência:
- usar um único ci.yml com jobs separados:
  - backend
  - frontend

3. Se substituir backend-ci.yml por ci.yml:
   - remover backend-ci.yml para evitar duplicidade de pipelines;
   - garantir que a documentação cite o novo ci.yml;
   - manter o histórico de decisão documentado.

4. O workflow deve rodar em:
   - push
   - pull_request

5. Job backend:
   - usar ubuntu-latest;
   - checkout do repositório;
   - setup .NET 8;
   - restore;
   - build;
   - test.

6. Backend deve rodar usando a solution:
   - src/backend/SpecPilot.sln

7. Backend commands esperados:
   - dotnet restore src/backend/SpecPilot.sln
   - dotnet build src/backend/SpecPilot.sln --no-restore --configuration Release
   - dotnet test src/backend/SpecPilot.sln --no-build --configuration Release

8. Se os testes de integração usam Testcontainers:
   - garantir compatibilidade com GitHub Actions;
   - não configurar PostgreSQL manualmente se Testcontainers já gerencia;
   - garantir que Docker esteja disponível no runner padrão;
   - documentar essa decisão.

9. Job frontend:
   - usar ubuntu-latest;
   - checkout do repositório;
   - setup Node.js;
   - usar Node.js 20;
   - instalar dependências;
   - buildar frontend;
   - rodar testes frontend.

10. Frontend working-directory:
   - src/frontend/specpilot-web

11. Frontend commands esperados:
   - npm ci
   - npm run build
   - npm test

Orientação importante sobre testes frontend:
12. Verifique o package.json do frontend.
13. Garanta que o comando usado no CI não entre em modo watch.
14. Se npm test estiver usando modo watch, ajuste para um comando próprio de CI.
15. Preferência:
   - manter npm test como vitest run, se já estiver assim;
   - ou criar script test:ci com vitest run e usar npm run test:ci no workflow.
16. Não use vitest sem run no CI, pois pode travar aguardando alterações.
17. Não use modo interativo no GitHub Actions.
18. Se criar test:ci, documente no README e docs/08-testing-strategy.md.

Regras de ambiente:
19. O CI não deve usar API key real.
20. O CI não deve fazer chamada real à OpenAI.
21. O CI não deve exigir backend real para testes frontend.
22. O CI não deve exigir frontend para testes backend.
23. O CI não deve executar docker compose up como parte obrigatória nesta etapa.
24. O CI deve ser rápido, simples e adequado ao MVP.

Restrições:
25. Não implementar deploy.
26. Não publicar Docker image.
27. Não adicionar cloud.
28. Não adicionar Playwright completo.
29. Não alterar backend, salvo se for correção mínima absolutamente necessária.
30. Não alterar frontend funcional, salvo ajustes necessários para CI/test script.
31. Não aumentar escopo do MVP.

Documentação:
32. Atualizar README.md adicionando ou ajustando seção de CI:
   - explicar que GitHub Actions valida backend e frontend;
   - explicar que o CI roda build e testes;
   - explicar que não há deploy nesta etapa;
   - citar o comando correto de testes frontend, especialmente se for criado test:ci.

33. Atualizar docs/08-testing-strategy.md:
   - incluir que backend e frontend são validados no CI;
   - citar testes backend e frontend;
   - explicar que testes frontend rodam sem backend real;
   - explicar que testes backend usam FakeAiService e não chamam OpenAI real.

34. Atualizar docs/13-development-best-practices.md se necessário:
   - citar integração contínua como prática de qualidade.

35. Atualizar docs/development-log.md:
   - adicionar entrada curta sobre o Prompt 16b.

36. Atualizar ADR existente de GitHub Actions, se houver:
   - docs/adr/0011-use-github-actions-for-ci.md

Ou criar uma ADR complementar se necessário.

A ADR deve deixar claro:
- CI valida backend e frontend.
- CI não faz deploy.
- CI não usa segredos reais.
- CI não chama OpenAI.
- CI mantém FakeAiService nos testes backend.
- CI usa testes frontend isolados do backend.

Rastreabilidade:
37. Salve este próprio prompt em:
   - prompts/codex/16b-update-github-actions-full-ci.md

38. Se o arquivo já existir, atualize-o com esta versão.

Validação local:
39. Rode localmente, se possível:
   - dotnet test src/backend/SpecPilot.sln

40. Rode em src/frontend/specpilot-web:
   - npm run build
   - npm test

41. Se criar test:ci, rode também:
   - npm run test:ci

42. Valide a sintaxe YAML do workflow, se possível.

43. Rode:
   - git status

Critérios de aceite:
- CI valida backend.
- CI valida frontend.
- Backend e frontend são jobs separados ou etapas claramente separadas.
- CI roda em push e pull_request.
- CI não faz deploy.
- CI não usa segredos reais.
- CI não faz chamada real à OpenAI.
- CI não trava em modo watch do Vitest.
- CI usa comando não interativo para testes frontend.
- Documentação atualizada.
- Prompt salvo em prompts/codex.
- Escopo do MVP não aumentou.

Não implemente nesta etapa:
- deploy;
- publicação de Docker image;
- Playwright completo;
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
ci: validate backend and frontend
