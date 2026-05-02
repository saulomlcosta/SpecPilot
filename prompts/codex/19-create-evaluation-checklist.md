Crie um checklist de avaliacao do MVP do SpecPilot AI.

Contexto:
Estamos no Prompt 19.
O projeto ja possui:
- backend funcional e testado;
- frontend funcional e testado;
- fluxo principal do MVP implementado;
- Docker Compose validado no Prompt 17;
- CI backend + frontend passando;
- Prompt 18 finalizado com polimento da jornada do usuario;
- FakeAiService como provider padrao;
- OpenAI provider opcional;
- documentacao e prompts rastreados no repositorio.

Ultimo commit relevante:
c06d1d0 fix(frontend): polish mvp user journey

Objetivo:
Criar um documento de checklist para avaliadores, facilitando a validacao do projeto de forma objetiva, didatica e reproduzivel.

Antes de comecar, leia obrigatoriamente:
- README.md
- AGENTS.md
- docs/02-mvp-scope.md
- docs/04-ai-usage.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- docs/10-setup-guide.md
- docs/12-docker-strategy.md
- docs/13-development-best-practices.md
- docs/manual-backend-validation.md
- docs/development-log.md
- prompts/codex/17-validate-docker-compose.md
- prompts/codex/18-polish-mvp-user-journey.md

Depois rode:
- git status
- git log --oneline -10

Tarefas:
1. Criar o arquivo:
   - docs/14-evaluation-checklist.md

2. O checklist deve ser escrito em portugues, com tom didatico e objetivo.

3. O checklist deve ajudar um avaliador a verificar se o projeto atende ao escopo do MVP, as praticas de engenharia e ao uso de IA Generativa.

Estrutura obrigatoria do arquivo:

# Checklist de Avaliacao do MVP

## Objetivo do checklist
Explique que o documento serve para orientar a validacao do MVP do SpecPilot AI.

## 1. Execucao local com Docker
Incluir checklist para:
- clonar o repositorio;
- copiar/configurar .env, se aplicavel;
- rodar docker compose up --build;
- acessar frontend em http://localhost:3000;
- acessar API em http://localhost:8080;
- acessar Swagger em http://localhost:8080/swagger;
- acessar health check em http://localhost:8080/health;
- confirmar que OpenAI nao e obrigatoria no modo padrao.

## 2. Funcionalidades do MVP
Incluir checklist para:
- criar conta;
- fazer login;
- fazer logout;
- criar projeto;
- listar projetos;
- visualizar detalhes;
- editar campos permitidos;
- excluir projeto;
- gerar perguntas de refinamento;
- responder perguntas;
- gerar documento tecnico;
- visualizar documento.

## 3. Fluxo principal esperado
Descrever o fluxo:
register/login -> create project -> generate questions -> answer questions -> generate document -> view document

Tambem listar as transicoes:
- Draft
- QuestionsGenerated
- QuestionsAnswered
- DocumentGenerated

## 4. Uso de IA Generativa
Incluir checklist para:
- confirmar que IA e usada para gerar perguntas;
- confirmar que IA e usada para gerar documento;
- confirmar que FakeAiService e o provider padrao;
- confirmar que OpenAI e opcional;
- confirmar que nao ha chamada direta a OpenAI pelo frontend;
- confirmar que prompts runtime estao documentados.

## 5. Prompts e CO-STAR
Incluir checklist para:
- verificar prompts em prompts/runtime;
- verificar prompts em prompts/codex;
- verificar uso do metodo CO-STAR nos prompts runtime;
- verificar que os prompts do Codex documentam o processo de desenvolvimento assistido por IA.

## 6. Testes automatizados
Incluir checklist para:
- rodar dotnet test src/backend/SpecPilot.sln;
- rodar npm ci no frontend;
- rodar npm run build no frontend;
- rodar npm test no frontend;
- verificar testes unitarios;
- verificar testes de integracao;
- verificar testes frontend.

## 7. CI com GitHub Actions
Incluir checklist para:
- verificar workflow em .github/workflows;
- confirmar que backend e validado;
- confirmar que frontend e validado;
- confirmar que CI nao faz deploy;
- confirmar que CI nao usa segredos reais;
- confirmar que CI nao chama OpenAI real.

## 8. Docker e Docker Compose
Incluir checklist para:
- verificar docker-compose.yml;
- verificar Dockerfile da API;
- verificar Dockerfile do frontend;
- confirmar postgres, api e frontend;
- confirmar Ai__Provider=Fake por padrao.

## 9. Seguranca basica
Incluir checklist para:
- JWT;
- rotas protegidas;
- usuario nao acessa projeto de outro usuario;
- status do projeto nao e editavel manualmente;
- senha nao e retornada pela API;
- API key nao e versionada;
- frontend nao contem chave OpenAI;
- erros nao expoem stack trace.

## 10. Boas praticas de desenvolvimento
Incluir checklist para:
- Result Pattern;
- ProblemDetails;
- Global Exception Handler;
- separacao de responsabilidades;
- validacao com FluentValidation no backend;
- validacao com Zod no frontend;
- TanStack Query no frontend;
- Conventional Commits;
- ADRs;
- development log.

## 11. Limitacoes conhecidas
Listar claramente:
- sem RAG;
- sem upload de arquivos;
- sem exportacao PDF;
- sem chat livre;
- sem multiplos agentes;
- sem deploy cloud;
- sem colaboracao multiusuario;
- OpenAI opcional, FakeAiService padrao.

## 12. Proximos passos possiveis
Listar como evolucao futura:
- RAG;
- upload de documentos;
- exportacao PDF;
- versionamento de documentos;
- multiplos agentes especializados;
- Playwright E2E;
- deploy cloud;
- observabilidade avancada.

## 13. Resultado esperado
Explicar de forma objetiva quando o projeto pode ser considerado aprovado no checklist.

4. Atualizar README.md adicionando um link para:
   - docs/14-evaluation-checklist.md

Sugestao:
Adicionar em uma secao de documentacao complementar ou avaliacao.

5. Atualizar docs/development-log.md adicionando entrada curta sobre o Prompt 19.

6. Salvar este proprio prompt em:
   - prompts/codex/19-create-evaluation-checklist.md

7. Nao implementar nenhuma funcionalidade.
8. Nao alterar backend.
9. Nao alterar frontend funcional.
10. Nao alterar Docker.
11. Nao reescrever README storytelling nesta etapa.
12. Nao aumentar escopo do MVP.

Validacao:
1. Rode:
   - git status

2. Se possivel, validar links relativos dos documentos alterados.

Criterios de aceite:
- docs/14-evaluation-checklist.md criado.
- Checklist e claro, didatico e util para avaliadores.
- README.md contem link para o checklist.
- docs/development-log.md atualizado.
- Prompt salvo em prompts/codex.
- Nenhuma funcionalidade foi alterada.
- Escopo do MVP permanece pequeno.

Ao finalizar:
1. Rode git status.
2. Liste arquivos alterados.
3. Faca commit usando Conventional Commits.

Mensagem de commit:
docs: add evaluation checklist
