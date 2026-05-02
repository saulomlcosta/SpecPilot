Implemente o fluxo de resposta das perguntas de refinamento.

Antes de começar, leia:

- AGENTS.md
- docs/02-mvp-scope.md
- docs/06-data-model.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- docs/13-development-best-practices.md

Objetivo:
Permitir que o usuário responda as perguntas geradas pela IA.

Endpoint:
- PUT /api/projects/{id}/questions/answers

Regras:
1. Apenas usuário autenticado pode responder perguntas.
2. Usuário só pode responder perguntas do próprio projeto.
3. Só permitir resposta se o projeto estiver com status QuestionsGenerated.
4. Exigir resposta para todas as perguntas.
5. Salvar as respostas no banco.
6. Atualizar status do projeto para QuestionsAnswered.
7. Usar MediatR.
8. Usar FluentValidation.
9. Criar testes unitários.
10. Criar testes de integração.
11. Atualizar documentação de API se necessário.
12. Salvar este próprio prompt em:
    - prompts/codex/08-implement-question-answers.md
13. Fazer commit ao final usando Conventional Commits.

Critérios de aceite:
- Usuário responde todas as perguntas de um projeto próprio.
- Respostas são persistidas.
- Status muda para QuestionsAnswered.
- Não é possível responder perguntas de projeto de outro usuário.
- Não é possível responder se não houver perguntas geradas.
- Não é possível enviar respostas incompletas.
- Testes passam.

Não implemente:
- Documento
- OpenAI real
- Frontend

Mensagem de commit:
feat(questions): implement question answering flow
