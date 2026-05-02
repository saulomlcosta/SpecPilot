Adicione GitHub Actions ao repositorio para validar o backend automaticamente.

Contexto:
Estamos desenvolvendo o SpecPilot AI, um MVP academico de pos-graduacao em IA Generativa.
O Prompt 12 foi finalizado e a cobertura de testes backend foi consolidada.
Queremos garantir que, a cada push ou pull request, o backend seja validado automaticamente.

Antes de comecar, leia:

- README.md
- AGENTS.md
- docs/08-testing-strategy.md
- docs/13-development-best-practices.md
- docs/development-log.md
- prompts/codex/12-consolidate-backend-tests.md

Depois rode:
- git status
- git log --oneline -10

Objetivo:
Criar uma pipeline simples de CI com GitHub Actions para rodar restore, build e testes do backend.

Tarefas:
1. Crie a pasta:
   - .github/workflows

2. Crie o arquivo:
   - .github/workflows/backend-ci.yml

3. Configure o workflow para rodar em:
   - push
   - pull_request

4. O workflow deve executar:
   - checkout do repositorio
   - setup do .NET 8
   - dotnet restore
   - dotnet build --no-restore --configuration Release
   - dotnet test --no-build --configuration Release

5. Use a solution do backend localizada em:
   - src/backend

6. Se os testes de integracao usam Testcontainers:
   - mantenha compatibilidade com o runner ubuntu-latest;
   - nao configure PostgreSQL manualmente se Testcontainers ja gerencia isso;
   - garanta que Docker esteja disponivel no runner;
   - documente essa decisao.

7. Se os testes de integracao dependem de PostgreSQL via service container:
   - configure um service container PostgreSQL no workflow;
   - documente essa decisao.

8. A pipeline nao deve fazer chamada real a OpenAI.
9. A pipeline deve usar FakeAiService.
10. Nao usar segredos reais no workflow.
11. Nao implementar deploy.
12. Nao publicar imagem Docker.
13. Nao adicionar cloud.
14. Nao aumentar escopo do MVP.

Documentacao:
1. Atualize README.md adicionando uma secao curta sobre CI:
   - explique que GitHub Actions valida build e testes automaticamente;
   - explique que isso apoia qualidade e confiabilidade do MVP.

2. Atualize docs/08-testing-strategy.md explicando que os testes backend tambem sao executados no CI.

3. Atualize docs/13-development-best-practices.md citando integracao continua como pratica de qualidade.

4. Atualize docs/development-log.md adicionando uma entrada curta sobre esta etapa.

5. Crie uma ADR:
   - docs/adr/0011-use-github-actions-for-ci.md

A ADR deve conter:
- Status: Aceita
- Contexto
- Decisao
- Consequencias positivas
- Consequencias negativas
- Alternativas consideradas

Rastreabilidade:
1. Crie ou atualize:
   - prompts/codex/12a-add-github-actions-ci.md

2. Salve este proprio prompt nesse arquivo.

Validacao:
1. Rode os testes localmente, se possivel:
   - dotnet test
2. Se possivel, valide a sintaxe do workflow.
3. Rode:
   - git status

Ao finalizar:
Faca commit usando Conventional Commits.

Mensagem de commit:
ci: add backend github actions workflow
