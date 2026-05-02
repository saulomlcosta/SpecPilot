Implemente o provider real de OpenAI como opção configurável, mantendo o FakeAiService como padrão para testes e execução local.

Antes de começar, leia:

- AGENTS.md
- docs/04-ai-usage.md
- docs/05-prompts.md
- docs/08-testing-strategy.md
- docs/09-security.md
- prompts/runtime/generate-refinement-questions.costar.md
- prompts/runtime/generate-project-document.costar.md

Objetivo:
Permitir usar IA real quando configurado, sem quebrar execução local ou testes.

Requisitos:
1. Criar OpenAiService implementando a mesma interface de IA.
2. Ler configurações por variáveis de ambiente:
   - Ai__Provider
   - Ai__OpenAi__ApiKey
   - Ai__OpenAi__Model
3. Manter Ai__Provider=Fake como padrão.
4. Quando Ai__Provider=OpenAI, usar OpenAiService.
5. Não versionar chave real.
6. Usar os prompts documentados em prompts/runtime.
7. Renderizar prompts substituindo placeholders.
8. Solicitar saída em JSON estruturado.
9. Validar o JSON antes de retornar para a aplicação.
10. Tratar erro de IA de forma controlada.
11. Registrar prompt, response, provider, model e metadados possíveis em AiInteractionLog.
12. Atualizar .env.example.
13. Atualizar README.md com instruções de uso Fake e OpenAI.
14. Atualizar docs/04-ai-usage.md se necessário.
15. Criar testes para seleção de provider e renderização de prompt.
16. Não fazer testes dependentes de chamada real à OpenAI.
17. Salvar este próprio prompt em:
    - prompts/codex/10-integrate-openai-provider.md
18. Fazer commit ao final usando Conventional Commits.

Critérios de aceite:
- Com Ai__Provider=Fake, aplicação funciona sem chave externa.
- Com Ai__Provider=OpenAI e chave válida, aplicação usa OpenAI.
- Sem chave, o modo Fake continua funcionando.
- Prompts runtime são usados pelo provider real.
- Respostas JSON são validadas.
- Testes passam sem depender da OpenAI.

Decisao registrada nesta etapa:
- usar HttpClient direto contra a API da OpenAI
- manter OpenAI opcional
- preservar FakeAiService como padrão
- encapsular HTTP apenas na Infrastructure
- usar IHttpClientFactory
- tratar timeout, rede, HTTP não 2xx e JSON inválido
- não usar SDK oficial da OpenAI nesta etapa

Não implemente:
- RAG
- Upload
- PDF
- Múltiplos agentes
- Chat livre

Mensagem de commit:
feat(ai): integrate openai provider
