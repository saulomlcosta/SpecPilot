Implemente a geracao de perguntas de refinamento usando a interface de IA existente e o FakeAiService.

Antes de comecar, leia:

- AGENTS.md
- docs/02-mvp-scope.md
- docs/04-ai-usage.md
- docs/06-data-model.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- prompts/runtime/generate-refinement-questions.costar.md

Objetivo:
Permitir que o usuario gere perguntas de refinamento para um projeto criado.

Endpoint:
- POST /api/projects/{id}/generate-questions

Regras:
1. Apenas usuario autenticado pode gerar perguntas.
2. Usuario so pode gerar perguntas para seus proprios projetos.
3. So permitir geracao se o projeto estiver com status Draft.
4. Chamar IA por meio da interface, nunca diretamente.
5. Salvar as perguntas geradas no banco.
6. Atualizar status do projeto para QuestionsGenerated.
7. Registrar a interacao em AiInteractionLog.
8. Nao chamar OpenAI real nesta etapa.
9. Criar testes unitarios.
10. Criar testes de integracao.
11. Atualizar documentacao de API se necessario.
12. Salvar este proprio prompt em:
    - prompts/codex/07-implement-refinement-questions.md
13. Fazer commit ao final usando Conventional Commits.

Criterios de aceite:
- Projeto Draft gera perguntas com FakeAiService.
- Perguntas sao persistidas.
- Status muda para QuestionsGenerated.
- AiInteractionLog e criado.
- Nao e possivel gerar perguntas para projeto de outro usuario.
- Nao e possivel gerar perguntas novamente se o status nao permitir.
- Testes passam.

Nao implemente:
- Resposta das perguntas
- Geracao de documento
- OpenAI real
- Frontend

Mensagem de commit:
feat(ai): generate refinement questions with fake service
