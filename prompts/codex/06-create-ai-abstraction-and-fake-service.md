Crie a abstracao de IA do SpecPilot AI e implemente um FakeAiService.

Antes de comecar, leia:

- AGENTS.md
- docs/04-ai-usage.md
- docs/05-prompts.md
- docs/08-testing-strategy.md
- docs/13-development-best-practices.md
- prompts/runtime/generate-refinement-questions.costar.md
- prompts/runtime/generate-project-document.costar.md

Objetivo:
Preparar a aplicacao para usar IA de forma testavel, sem depender inicialmente de provedor externo.

Tarefas:
1. Criar uma interface para o servico de IA.
2. A interface deve suportar:
   - Gerar perguntas de refinamento
   - Gerar documento tecnico inicial
3. Criar modelos de request e response para IA.
4. Criar FakeAiService com respostas fixas e previsiveis.
5. Configurar selecao do provider por variavel de ambiente:
   - Ai__Provider=Fake
6. Garantir que FakeAiService seja o padrao em ambiente local e Docker.
7. Criar testes unitarios para FakeAiService.
8. Atualizar .env.example se necessario.
9. Atualizar docs/04-ai-usage.md se necessario.
10. Atualizar docs/08-testing-strategy.md se necessario.
11. Salvar este proprio prompt em:
    - prompts/codex/06-create-ai-abstraction-and-fake-service.md
12. Fazer commit ao final usando Conventional Commits.

Regras:
- Nao integrar OpenAI ainda.
- Nao fazer chamada externa nesta etapa.
- Nao implementar endpoints de geracao ainda.
- Nao adicionar RAG.
- Nao adicionar multiplos agentes.
- Nao adicionar chat livre.
- Manter a IA isolada atras de interface.

Criterios de aceite:
- A aplicacao possui uma interface de IA.
- Existe FakeAiService funcional.
- O provider Fake pode ser configurado por variavel de ambiente.
- Os testes passam.

Mensagem de commit:
feat(ai): add fake ai provider
