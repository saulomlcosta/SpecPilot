Estou iniciando um projeto academico de pos-graduacao em IA Generativa chamado SpecPilot AI.

Quero que voce prepare o repositorio do zero, com foco didatico, escopo pequeno, alta qualidade, execucao com Docker e boas praticas de engenharia de software.

Objetivo do projeto:
O SpecPilot AI e uma aplicacao web onde o usuario cria um projeto de software, descreve a ideia inicial, recebe perguntas de refinamento geradas por IA e, apos responder essas perguntas, recebe uma documentacao tecnica inicial.

MVP permitido:
- Cadastro de usuario
- Login simples
- Criacao de projeto
- Descricao inicial da ideia do sistema
- Geracao de perguntas de refinamento com IA
- Resposta das perguntas pelo usuario
- Geracao de documento inicial com:
  - Visao geral
  - Requisitos funcionais
  - Requisitos nao funcionais
  - Casos de uso
  - Riscos

Requisitos tecnicos obrigatorios:
- Backend em .NET 8 com ASP.NET Core Web API
- Frontend em React com TypeScript
- PostgreSQL
- Docker
- Docker Compose
- Testes unitarios
- Testes de integracao
- FakeAiService para permitir testes e execucao sem chave externa de IA
- OpenAI provider opcional via variavel de ambiente
- README com instrucoes para o avaliador rodar o projeto
- Conventional Commits
- Prompts documentados
- ADRs documentadas
- Boas praticas: SOLID, DRY, KISS, YAGNI, Clean Code, validacoes e separacao de responsabilidades

Fora do MVP:
- RAG
- Upload de arquivos
- PDF
- Chat livre
- Microservicos
- Kafka
- RabbitMQ
- Multiplos agentes
- Integracao com GitHub
- Geracao de codigo pelo sistema
- Dashboard complexo
- Multiusuario colaborativo

Tarefas desta etapa:
1. Inicialize o repositorio Git se ainda nao estiver inicializado.
2. Crie a estrutura inicial de documentacao.
3. Crie uma pasta de prompts separando:
   - prompts/runtime
   - prompts/codex
4. Salve este proprio prompt em:
   - prompts/codex/00-create-documentation-base.md
5. Crie README.md didatico com diagramas Mermaid.
6. Crie AGENTS.md com regras para o Codex seguir durante o projeto.
7. Crie documentacao em docs/.
8. Crie ADRs iniciais.
9. Documente estrategia de Docker e Docker Compose.
10. Documente estrategia de testes unitarios e integrados.
11. Documente boas praticas de desenvolvimento.
12. Nao implemente backend ainda.
13. Nao implemente frontend ainda.
14. Ao final, faca um commit usando Conventional Commits.

Estrutura esperada:

- README.md
- AGENTS.md
- .gitignore
- .env.example
- docker-compose.yml
- docs/00-project-overview.md
- docs/01-problem-statement.md
- docs/02-mvp-scope.md
- docs/03-architecture.md
- docs/04-ai-usage.md
- docs/05-prompts.md
- docs/06-data-model.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- docs/09-security.md
- docs/10-setup-guide.md
- docs/11-codex-development-process.md
- docs/12-docker-strategy.md
- docs/13-development-best-practices.md
- docs/adr/0001-use-monolithic-architecture.md
- docs/adr/0002-use-postgresql.md
- docs/adr/0003-use-structured-ai-output.md
- docs/adr/0004-use-costar-prompting.md
- docs/adr/0005-use-codex-as-development-agent.md
- docs/adr/0006-use-docker-compose.md
- docs/adr/0007-use-automated-tests.md
- docs/adr/0008-use-fake-ai-service-for-tests.md
- prompts/runtime/generate-refinement-questions.costar.md
- prompts/runtime/generate-project-document.costar.md
- prompts/codex/00-create-documentation-base.md
- prompts/codex/01-review-documentation.md
- prompts/codex/02-create-backend-skeleton.md
- prompts/codex/03-add-docker-support.md

Regras gerais:
- Escreva tudo em portugues.
- Seja didatico, pois sera avaliado em uma pos-graduacao.
- Use Conventional Commits.
- O README deve explicar o projeto para alguem que nunca viu o codigo.
- O README deve conter pelo menos:
  - Descricao do projeto
  - Problema
  - Objetivo
  - Escopo do MVP
  - Onde a IA e usada
  - Como o avaliador executa com Docker Compose
  - Como rodar testes
  - Arquitetura com Mermaid
  - Fluxo principal com Mermaid
  - Tecnologias previstas
  - Estrategia de testes
  - Decisoes de arquitetura
- A documentacao deve explicar onde a IA Generativa e usada e por que.
- Os prompts runtime devem seguir o metodo CO-STAR.
- Os prompts do Codex devem documentar o processo de desenvolvimento assistido por IA.
- A documentacao deve deixar claro que o projeto pode rodar com FakeAiService sem chave externa.
- Nao adicione funcionalidades fora do MVP.
- Nao implemente codigo de aplicacao nesta etapa.
- Faca commit ao final com a mensagem:
  docs: add initial project documentation

