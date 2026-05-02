Implemente a geração do documento técnico inicial usando a interface de IA existente e o FakeAiService.

Antes de começar, leia:

- AGENTS.md
- docs/02-mvp-scope.md
- docs/04-ai-usage.md
- docs/06-data-model.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- prompts/runtime/generate-project-document.costar.md

Objetivo:
Permitir que o usuário gere uma documentação técnica inicial depois de responder as perguntas de refinamento.

Endpoint:
- POST /api/projects/{id}/generate-document
- GET /api/projects/{id}/document

Documento gerado deve conter:
- Overview
- FunctionalRequirements
- NonFunctionalRequirements
- UseCases
- Risks

Regras:
1. Apenas usuário autenticado pode gerar documento.
2. Usuário só pode gerar documento para seus próprios projetos.
3. Só permitir geração se o projeto estiver com status QuestionsAnswered.
4. Chamar IA por meio da interface.
5. Salvar o documento no banco.
6. Atualizar status do projeto para DocumentGenerated.
7. Registrar a interação em AiInteractionLog.
8. Criar endpoint para buscar o documento gerado.
9. Criar testes unitários.
10. Criar testes de integração.
11. Atualizar documentação de API se necessário.
12. Salvar este próprio prompt em:
    - prompts/codex/09-implement-fake-ai-document.md
13. Fazer commit ao final usando Conventional Commits.

Critérios de aceite:
- Usuário consegue gerar documento após responder perguntas.
- Documento é persistido.
- Status muda para DocumentGenerated.
- AiInteractionLog é criado.
- Usuário consegue buscar documento gerado.
- Não é possível gerar documento antes de responder perguntas.
- Não é possível acessar documento de outro usuário.
- Testes passam.

Não implemente:
- Exportação PDF
- RAG
- Chat livre
- OpenAI real
- Frontend

Mensagem de commit:
feat(ai): generate project document with fake service
