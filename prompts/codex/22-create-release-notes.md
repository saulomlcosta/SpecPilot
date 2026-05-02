Crie um documento de release notes academico para a versao final do MVP do SpecPilot AI.

Contexto:
Estamos no Prompt 22.
O projeto ja passou por:
- backend funcional e testado;
- frontend funcional e testado;
- fluxo principal do MVP completo;
- Docker Compose validado;
- GitHub Actions validando backend e frontend;
- README storytelling final;
- revisao final contra escopo no Prompt 21;
- documentacao, ADRs, prompts e development log versionados.

Objetivo:
Criar um documento de release notes para registrar oficialmente o que foi entregue na versao MVP, com foco academico, tecnico e didatico.

Antes de comecar, leia obrigatoriamente:
- README.md
- docs/02-mvp-scope.md
- docs/04-ai-usage.md
- docs/08-testing-strategy.md
- docs/10-setup-guide.md
- docs/12-docker-strategy.md
- docs/13-development-best-practices.md
- docs/14-evaluation-checklist.md
- docs/development-log.md
- docs/adr/
- prompts/codex/20-final-storytelling-readme.md
- prompts/codex/21-final-scope-review.md, se existir

Depois rode:
- git status
- git log --oneline -20

Tarefas:
1. Criar o arquivo:
   - docs/15-release-notes.md

2. Escrever em portugues, com tom tecnico, didatico e profissional.

3. O documento deve conter a estrutura:

# Release Notes — SpecPilot AI MVP

## Versao
Use:
- MVP 1.0

## Data
Use a data atual do sistema ou a data de execucao.

## Resumo da entrega
Explique em poucas linhas o que foi entregue.

## Funcionalidades implementadas
Listar:
- cadastro;
- login;
- logout;
- gerenciamento de projetos;
- geracao de perguntas de refinamento;
- resposta das perguntas;
- geracao de documento tecnico;
- visualizacao do documento.

## Uso de IA Generativa
Explicar:
- FakeAiService como padrao;
- OpenAI provider opcional;
- prompts CO-STAR;
- JSON estruturado;
- AiInteractionLog;
- backend centralizando IA;
- frontend nao chama OpenAI diretamente.

## Uso de IA no desenvolvimento
Explicar:
- Codex como agente auxiliar;
- prompts versionados em prompts/codex;
- human-in-the-loop;
- development log;
- ADRs;
- Conventional Commits;
- uso de skills/superpowers como brainstorming, debugging, TDD e verification-before-completion quando aplicavel.

## Entregas tecnicas
Listar:
- backend .NET;
- frontend React;
- PostgreSQL;
- Docker Compose;
- CI com GitHub Actions;
- testes backend;
- testes frontend;
- documentacao;
- ADRs;
- prompts runtime;
- prompts Codex.

## Testes e validacao
Explicar:
- dotnet test;
- npm ci;
- npm run build;
- npm test;
- CI backend/frontend;
- Docker Compose validado;
- fluxo principal validado.

## Decisoes arquiteturais relevantes
Resumir:
- arquitetura em camadas;
- PostgreSQL;
- Result Pattern + ProblemDetails;
- Global Exception Handler;
- FakeAiService;
- OpenAI provider opcional via HttpClient;
- protecao de ProjectStatus;
- Docker Compose;
- GitHub Actions;
- prompts CO-STAR.

## Limitacoes conhecidas
Listar:
- sem RAG;
- sem upload;
- sem PDF;
- sem chat livre;
- sem multiplos agentes;
- sem deploy cloud;
- sem colaboracao multiusuario;
- sem Playwright E2E completo, se realmente nao foi implementado.

## Proximos passos possiveis
Listar:
- RAG;
- upload de arquivos;
- exportacao PDF;
- versionamento de documentos;
- multiplos agentes especializados;
- Playwright E2E;
- deploy cloud;
- observabilidade avancada.

## Historico resumido de commits por etapa
Use git log para criar uma lista resumida dos commits principais.
Nao precisa listar todos se ficar grande demais.
Agrupe por temas:
- documentacao inicial;
- backend;
- IA;
- testes;
- frontend;
- CI;
- Docker;
- README final.

## Como validar a release
Incluir comandos:
- docker compose up --build
- dotnet test src/backend/SpecPilot.sln
- npm ci
- npm run build
- npm test

## Resultado final
Explique que a versao MVP esta pronta para avaliacao academica e demonstra uso aplicado de IA Generativa com engenharia, testes, Docker, CI e documentacao.

4. Atualizar README.md adicionando link para:
   - docs/15-release-notes.md

5. Atualizar docs/development-log.md adicionando entrada curta sobre o Prompt 22.

6. Salvar este proprio prompt em:
   - prompts/codex/22-create-release-notes.md

Restricoes:
1. Nao implementar funcionalidade.
2. Nao alterar backend.
3. Nao alterar frontend.
4. Nao alterar Docker.
5. Nao alterar workflow.
6. Nao aumentar escopo.
7. Nao reescrever README inteiro.

Validacao:
1. Validar links relativos, se possivel.
2. Rodar git status.

Criterios de aceite:
- docs/15-release-notes.md criado.
- README contem link para release notes.
- development-log atualizado.
- prompt salvo em prompts/codex.
- Nenhum codigo funcional alterado.
- Escopo continua coerente.

Ao finalizar:
1. Rode git status.
2. Liste arquivos alterados.
3. Faca commit usando Conventional Commits.

Mensagem de commit:
docs: add mvp release notes
