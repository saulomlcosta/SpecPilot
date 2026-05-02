Reescreva o README.md final do projeto em formato storytelling, didatico e profissional, adequado para uma entrega de pos-graduacao em IA Generativa.

Contexto:
Estamos no Prompt 20.
O projeto SpecPilot AI ja possui:
- backend funcional e testado;
- frontend funcional e testado;
- fluxo principal do MVP implementado;
- Docker Compose validado;
- CI backend + frontend passando;
- checklist de avaliacao criado no Prompt 19;
- documentacao em docs/;
- ADRs;
- prompts runtime;
- prompts Codex;
- development log;
- FakeAiService como provider padrao;
- OpenAI provider opcional.

Objetivo:
Criar um README final que conte a historia do projeto, explique as decisoes tecnicas e mostre claramente:
1. como a IA Generativa e usada dentro do produto;
2. como a IA tambem foi usada durante o processo de desenvolvimento com Codex, prompts versionados, human-in-the-loop e skills/superpowers.

Antes de comecar, leia obrigatoriamente:
- README.md
- AGENTS.md
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
- docs/14-evaluation-checklist.md
- docs/manual-backend-validation.md
- docs/development-log.md
- docs/adr/
- prompts/runtime/
- prompts/codex/

Depois rode:
- git status
- git log --oneline -10

Tarefas:
1. Reescrever README.md em portugues.
2. Usar tom didatico, profissional e claro.
3. Nao inventar funcionalidades que nao existem.
4. Nao prometer recursos nao implementados.
5. Nao alterar codigo.
6. Nao alterar backend.
7. Nao alterar frontend.
8. Nao alterar Docker.
9. Nao aumentar escopo.

Estrutura obrigatoria do README:

# SpecPilot AI

## Resumo
Explique em poucas linhas o que e o projeto.

## A historia do projeto
Conte o problema de forma didatica:
- ideias de software comecam vagas;
- requisitos nem sempre estao claros;
- documentacao inicial costuma ser negligenciada;
- IA Generativa pode apoiar o refinamento inicial.

## Problema
Explique o problema que o projeto resolve.

## Objetivo academico
Explique que o projeto foi desenvolvido como MVP academico de pos-graduacao em IA Generativa.

## O que o SpecPilot AI faz
Explique o fluxo:
1. usuario cria conta;
2. cria projeto;
3. descreve ideia;
4. IA gera perguntas;
5. usuario responde;
6. IA gera documento tecnico;
7. usuario visualiza documento.

## Escopo do MVP
Liste o que esta incluido:
- autenticacao;
- projetos;
- perguntas de refinamento;
- respostas;
- geracao de documento;
- visualizacao do documento;
- Docker;
- testes;
- CI;
- documentacao;
- prompts versionados.

## Fora do escopo do MVP
Liste claramente:
- RAG;
- upload de arquivos;
- exportacao PDF;
- chat livre;
- multiplos agentes;
- microservicos;
- Kafka/RabbitMQ;
- deploy cloud;
- colaboracao multiusuario.

Explique que ficaram fora para manter o MVP pequeno, testavel e bem documentado.

## Duas dimensoes de uso de IA
Explique que o projeto usa IA em duas dimensoes:
1. IA dentro do produto;
2. IA durante o desenvolvimento.

Deixe claro que isso foi proposital para demonstrar nao so uma funcionalidade com IA, mas tambem um processo de engenharia assistido por IA e controlado por humano.

## IA dentro do produto
Explique:
- geracao de perguntas de refinamento;
- geracao do documento tecnico inicial;
- FakeAiService como padrao;
- OpenAI provider opcional;
- frontend nunca chama OpenAI diretamente;
- backend centraliza integracao com IA;
- prompts runtime em CO-STAR;
- AiInteractionLog para rastreabilidade;
- JSON estruturado e validacao.

## Como a IA foi controlada dentro do produto
Explique:
- prompts CO-STAR;
- JSON estruturado;
- validacao;
- Result Pattern;
- ProblemDetails;
- AiInteractionLog;
- FakeAiService;
- revisao humana;
- documentacao das decisoes.

## IA no desenvolvimento: Codex, prompts e human-in-the-loop
Esta secao e obrigatoria e deve ser bem explicada.

Explique:
- Codex foi usado como agente auxiliar de desenvolvimento.
- O desenvolvimento nao foi feito com um prompt unico do tipo "crie tudo".
- O projeto foi dividido em prompts sequenciais e pequenos.
- Cada prompt tinha contexto, objetivo, tarefas, criterios de aceite, validacoes e mensagem de commit.
- Os prompts foram versionados em prompts/codex.
- O Codex era orientado a ler README.md, AGENTS.md e docs relevantes antes de implementar.
- O AGENTS.md funcionou como guia de comportamento do agente.
- O docs/development-log.md registrou marcos, problemas, decisoes e retomadas de contexto.
- As ADRs registraram decisoes arquiteturais.
- O humano atuou como revisor, decisor e limitador de escopo.
- O humano validou decisoes como Result Pattern, ProblemDetails, Docker, GitHub Actions, controle de ProjectStatus e escopo do MVP.
- Commits foram feitos por etapa usando Conventional Commits.
- O GitHub Actions validou continuamente backend e frontend.
- Docker Compose foi usado para garantir reprodutibilidade.

Tambem explique que durante a execucao dos prompts no Codex foram usadas skills/superpowers do ambiente, como:
- brainstorming, para desenhar abordagens antes de implementar;
- systematic debugging, para investigar falhas como o problema do npm ci/package-lock no CI;
- test-driven-development quando aplicavel, especialmente em fluxos de backend e testes;
- verification-before-completion, para validar antes de concluir etapas.

Explique que essas skills/superpowers foram apoio metodologico e nao substituiram a decisao humana.

Deixe claro:
- a IA sugeria, implementava e validava;
- o humano revisava, aprovava, corrigia direcao e controlava o escopo;
- decisoes relevantes eram documentadas;
- a memoria permanente do projeto ficou no repositorio, nao na conversa.

Inclua um diagrama Mermaid mostrando o processo de desenvolvimento assistido por IA:

flowchart LR
    Human[Humano / Revisor] --> Prompt[Prompt versionado]
    Prompt --> Codex[Codex]
    Codex --> Changes[Codigo, testes e documentacao]
    Changes --> Validation[Validacao: testes, Docker e CI]
    Validation --> Commit[Commit convencional]
    Commit --> Next[Proxima etapa]
    Human --> Validation

Explique que essa abordagem foi usada para evitar:
- escopo descontrolado;
- decisoes nao documentadas;
- dependencia da memoria da conversa;
- codigo sem teste;
- entregas dificeis de reproduzir.

## Metodo CO-STAR
Explique brevemente:
- Context
- Objective
- Style
- Tone
- Audience
- Response

Aponte para prompts/runtime.

## Arquitetura
Inclua um diagrama Mermaid da arquitetura geral:

Frontend React
↓
ASP.NET Core API
↓
Application/Domain/Infrastructure
↓
PostgreSQL
↓
AI Provider Fake/OpenAI

## Fluxo principal
Inclua um diagrama Mermaid do fluxo do usuario.

## Fluxo de geracao de perguntas
Inclua um diagrama Mermaid especifico.

## Fluxo de geracao de documento
Inclua um diagrama Mermaid especifico.

## Tecnologias utilizadas
Separar por:
- Backend;
- Frontend;
- IA;
- Banco;
- Infra;
- Testes;
- CI.

## Como executar com Docker Compose
Incluir comandos reais:
- git clone
- cd
- cp .env.example .env, se aplicavel
- docker compose up --build

Listar URLs:
- Frontend: http://localhost:3000
- API: http://localhost:8080
- Swagger: http://localhost:8080/swagger
- Health: http://localhost:8080/health

## Como executar sem Docker
Se ja estiver documentado, incluir comandos basicos:
- backend dotnet run;
- frontend npm install/npm run dev.

Nao inventar se nao estiver suportado.

## Variaveis de ambiente
Explicar:
- Ai__Provider=Fake
- Ai__OpenAi__ApiKey opcional
- Ai__OpenAi__Model opcional
- JWT
- PostgreSQL
- VITE_API_BASE_URL

Deixar claro:
- OpenAI nao e obrigatoria;
- FakeAiService e padrao;
- nao versionar segredos.

## Como rodar os testes
Incluir:
- dotnet test src/backend/SpecPilot.sln
- npm ci
- npm run build
- npm test

## CI com GitHub Actions
Explicar:
- backend e frontend sao validados;
- build e testes rodam no CI;
- nao ha deploy;
- nao usa segredos reais;
- nao chama OpenAI real.

## Estrutura de pastas
Mostrar arvore resumida:
- docs/
- docs/adr/
- prompts/runtime/
- prompts/codex/
- src/backend/
- src/frontend/
- tests/backend/

## Decisoes de arquitetura
Resumir principais decisoes:
- monolito em camadas no backend;
- PostgreSQL;
- Result Pattern + ProblemDetails;
- Global Exception Handler;
- FakeAiService;
- HttpClient para OpenAI;
- Docker Compose;
- GitHub Actions;
- protecao de ProjectStatus;
- prompts CO-STAR;
- human-in-the-loop no processo de desenvolvimento.

Linkar para docs/adr.

## Boas praticas aplicadas
Explicar:
- SOLID;
- DRY;
- KISS;
- YAGNI;
- Clean Code;
- validacao;
- testes;
- CI;
- separacao de responsabilidades;
- documentacao;
- Conventional Commits;
- development log;
- ADRs.

## Seguranca basica
Explicar:
- JWT;
- rotas protegidas;
- usuario nao acessa projeto de outro usuario;
- senha nao e retornada;
- OpenAI key nao vai para frontend;
- erros nao expoem stack trace;
- frontend nao chama OpenAI diretamente.

## Checklist de avaliacao
Adicionar link para:
- docs/14-evaluation-checklist.md

## Limitacoes conhecidas
Listar limitacoes reais.

## Proximos passos
Listar evolucoes futuras:
- RAG;
- upload;
- PDF;
- versionamento de documentos;
- multiplos agentes;
- Playwright E2E;
- deploy;
- observabilidade avancada.

## Conclusao
Fechar explicando que o projeto demonstra uso aplicado de IA Generativa com controle, rastreabilidade, testes, Docker, CI, documentacao e desenvolvimento assistido por IA com revisao humana.

Requisitos especificos:
1. README deve conter Mermaid valido para GitHub.
2. README deve apontar para documentos importantes.
3. README deve explicar onde a IA entra e onde ela nao entra.
4. README deve deixar claro que o projeto roda com FakeAiService sem chave externa.
5. README deve explicar como ativar OpenAI opcionalmente.
6. README deve mencionar que o frontend nao chama OpenAI diretamente.
7. README deve deixar claro que o escopo foi propositalmente pequeno.
8. README deve explicar como a IA foi usada durante o desenvolvimento.
9. README deve explicar a estrategia de prompts versionados em prompts/codex.
10. README deve citar human-in-the-loop.
11. README deve citar uso de skills/superpowers como apoio metodologico quando aplicavel.
12. README nao deve dizer que algo esta pendente se ja foi implementado.
13. README nao deve dizer que algo foi implementado se nao existe.
14. README deve ser util para avaliador e para portfolio.

Documentacao:
1. Atualizar docs/development-log.md adicionando entrada curta sobre o Prompt 20.
2. Salvar este proprio prompt em:
   - prompts/codex/20-final-storytelling-readme.md

Validacao:
1. Validar links relativos do README, se possivel.
2. Validar Mermaid visualmente ou pelo menos sintaticamente.
3. Rodar git status.

Criterios de aceite:
- README final esta didatico e completo.
- README conta a historia do projeto.
- README explica IA dentro do produto.
- README explica IA durante o desenvolvimento.
- README explica Codex, prompts, human-in-the-loop e superpowers/skills.
- README explica Docker, testes, CI e decisoes.
- README nao aumenta escopo.
- README nao inventa funcionalidade.
- development-log atualizado.
- prompt salvo em prompts/codex.
- Nenhum codigo funcional alterado.

Ao finalizar:
1. Rode git status.
2. Liste arquivos alterados.
3. Faca commit usando Conventional Commits.

Mensagem de commit:
docs: write final storytelling readme
