# Checklist de Avaliacao do MVP

## Objetivo do checklist

Este documento orienta a validacao do MVP do SpecPilot AI de forma objetiva, didatica e reproduzivel. A ideia e ajudar avaliadores a confirmar se o projeto atende ao escopo proposto, se o fluxo principal funciona e se as decisoes de engenharia e de uso de IA Generativa estao coerentes com a documentacao do repositorio.

## 1. Execucao local com Docker

- [ ] Clonar o repositorio em uma maquina com Git, Docker e Docker Compose.
- [ ] Copiar `.env.example` para `.env`.
- [ ] Rodar `docker compose up --build`.
- [ ] Confirmar que o frontend responde em `http://localhost:3000`.
- [ ] Confirmar que a API responde em `http://localhost:8080`.
- [ ] Confirmar que o Swagger responde em `http://localhost:8080/swagger`.
- [ ] Confirmar que o health check responde em `http://localhost:8080/health`.
- [ ] Confirmar que a execucao padrao nao exige chave OpenAI.
- [ ] Confirmar que `Ai__Provider=Fake` e suficiente para avaliacao local.

## 2. Funcionalidades do MVP

- [ ] Criar conta pela interface.
- [ ] Fazer login com a conta criada.
- [ ] Fazer logout.
- [ ] Criar projeto com nome, descricao inicial, objetivo e publico-alvo.
- [ ] Listar projetos do usuario autenticado.
- [ ] Visualizar detalhes de um projeto.
- [ ] Editar apenas os campos permitidos do projeto.
- [ ] Excluir um projeto.
- [ ] Gerar perguntas de refinamento.
- [ ] Responder perguntas de refinamento.
- [ ] Gerar documento tecnico inicial.
- [ ] Visualizar documento tecnico gerado.

## 3. Fluxo principal esperado

Fluxo esperado:

`register/login -> create project -> generate questions -> answer questions -> generate document -> view document`

Transicoes esperadas de status:

- [ ] `Draft`
- [ ] `QuestionsGenerated`
- [ ] `QuestionsAnswered`
- [ ] `DocumentGenerated`

Confirmacoes adicionais:

- [ ] O projeto nasce em `Draft`.
- [ ] Gerar perguntas move o projeto para `QuestionsGenerated`.
- [ ] Responder perguntas move o projeto para `QuestionsAnswered`.
- [ ] Gerar documento move o projeto para `DocumentGenerated`.
- [ ] Acoes incompativeis com o status atual nao ficam disponiveis como fluxo principal.

## 4. Uso de IA Generativa

- [ ] Confirmar que a IA e usada para gerar perguntas de refinamento.
- [ ] Confirmar que a IA e usada para gerar o documento tecnico inicial.
- [ ] Confirmar que `FakeAiService` e o provider padrao.
- [ ] Confirmar que OpenAI e opcional por configuracao.
- [ ] Confirmar que o frontend nao faz chamada direta para a OpenAI.
- [ ] Confirmar que os prompts de runtime estao documentados em `prompts/runtime/`.

## 5. Prompts e CO-STAR

- [ ] Verificar prompts de runtime em `prompts/runtime/`.
- [ ] Verificar prompts de desenvolvimento em `prompts/codex/`.
- [ ] Confirmar que os prompts runtime seguem o metodo CO-STAR.
- [ ] Confirmar que os prompts do Codex registram o processo de desenvolvimento assistido por IA por etapa.

## 6. Testes automatizados

- [ ] Rodar `dotnet test src/backend/SpecPilot.sln`.
- [ ] Rodar `npm ci` em `src/frontend/specpilot-web`.
- [ ] Rodar `npm run build` em `src/frontend/specpilot-web`.
- [ ] Rodar `npm test` em `src/frontend/specpilot-web`.
- [ ] Confirmar presenca de testes unitarios no backend.
- [ ] Confirmar presenca de testes de integracao no backend.
- [ ] Confirmar presenca de testes do frontend para fluxo visivel ao usuario.

## 7. CI com GitHub Actions

- [ ] Verificar workflow em `.github/workflows/ci.yml`.
- [ ] Confirmar que o backend e validado no CI.
- [ ] Confirmar que o frontend e validado no CI.
- [ ] Confirmar que o CI nao faz deploy.
- [ ] Confirmar que o CI nao usa segredos reais de OpenAI.
- [ ] Confirmar que o CI fixa `Ai__Provider=Fake` no backend.
- [ ] Confirmar que o CI nao chama OpenAI real.

## 8. Docker e Docker Compose

- [ ] Verificar `docker-compose.yml`.
- [ ] Verificar Dockerfile da API.
- [ ] Verificar Dockerfile do frontend.
- [ ] Confirmar presenca dos servicos `postgres`, `api` e `frontend`.
- [ ] Confirmar que `Ai__Provider=Fake` esta configurado por padrao.

## 9. Seguranca basica

- [ ] Confirmar uso de JWT para autenticacao.
- [ ] Confirmar existencia de rotas protegidas.
- [ ] Confirmar que um usuario nao acessa projeto de outro usuario.
- [ ] Confirmar que o status do projeto nao e editavel manualmente no fluxo comum.
- [ ] Confirmar que a senha nao e retornada pela API.
- [ ] Confirmar que nenhuma API key sensivel esta versionada.
- [ ] Confirmar que o frontend nao contem chave OpenAI embutida.
- [ ] Confirmar que erros HTTP nao expõem stack trace ao usuario.

## 10. Boas praticas de desenvolvimento

- [ ] Confirmar uso de Result Pattern para falhas esperadas no backend.
- [ ] Confirmar uso de `ProblemDetails` para respostas HTTP de erro.
- [ ] Confirmar existencia de handler global de excecoes.
- [ ] Confirmar separacao de responsabilidades entre camadas.
- [ ] Confirmar validacao com FluentValidation no backend.
- [ ] Confirmar validacao com Zod no frontend.
- [ ] Confirmar uso de TanStack Query no frontend.
- [ ] Confirmar uso de Conventional Commits.
- [ ] Confirmar existencia de ADRs em `docs/adr/`.
- [ ] Confirmar registro de evolucao em `docs/development-log.md`.

## 11. Limitacoes conhecidas

Itens fora do escopo atual:

- [ ] Sem RAG.
- [ ] Sem upload de arquivos.
- [ ] Sem exportacao PDF.
- [ ] Sem chat livre.
- [ ] Sem multiplos agentes.
- [ ] Sem deploy cloud.
- [ ] Sem colaboracao multiusuario.
- [ ] OpenAI opcional, com `FakeAiService` como padrao.

## 12. Proximos passos possiveis

Evolucoes futuras possiveis, sem fazer parte do MVP atual:

- [ ] RAG.
- [ ] Upload de documentos.
- [ ] Exportacao PDF.
- [ ] Versionamento de documentos.
- [ ] Multiplos agentes especializados.
- [ ] Playwright E2E.
- [ ] Deploy cloud.
- [ ] Observabilidade avancada.

## 13. Resultado esperado

O projeto pode ser considerado aprovado neste checklist quando:

- o ambiente sobe localmente com Docker Compose;
- o fluxo principal do MVP funciona de ponta a ponta;
- o uso de IA Generativa aparece apenas nos pontos previstos;
- testes automatizados e CI estao coerentes com a documentacao;
- as praticas de engenharia prometidas no repositorio podem ser verificadas;
- nao ha evidencia de ampliacao indevida de escopo alem do MVP academico.
