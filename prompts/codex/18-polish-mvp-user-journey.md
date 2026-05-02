Realize o polimento final do fluxo principal do MVP do SpecPilot AI.

Contexto:
O projeto ja possui:
- autenticacao funcional;
- gerenciamento de projetos funcional;
- fluxo de IA funcional;
- geracao de perguntas;
- resposta de perguntas;
- geracao de documento;
- visualizacao de documento;
- backend testado;
- frontend testado;
- Docker Compose validado;
- CI backend + frontend passando;
- FakeAiService como provider padrao;
- OpenAI provider opcional.

Objetivo:
Melhorar a experiencia do usuario, corrigir pequenos problemas de usabilidade, revisar mensagens, estados de loading/erro/sucesso e garantir que o fluxo principal esteja claro para avaliadores, sem adicionar novas funcionalidades.

Fluxo principal a revisar:
1. Criar conta.
2. Fazer login.
3. Criar projeto.
4. Abrir detalhes do projeto.
5. Gerar perguntas de refinamento.
6. Responder perguntas.
7. Gerar documento.
8. Visualizar documento.
9. Fazer logout.
10. Tentar acessar rota protegida sem autenticacao.

Tarefas de revisao da UI:
1. Revisar textos das telas para ficarem claros e didaticos.
2. Revisar botoes de acao:
   - textos claros;
   - estados disabled durante loading;
   - feedback apos sucesso;
   - feedback apos erro.
3. Revisar estados de loading em:
   - login;
   - cadastro;
   - listagem de projetos;
   - criacao de projeto;
   - detalhe de projeto;
   - geracao de perguntas;
   - envio de respostas;
   - geracao de documento;
   - carregamento do documento.
4. Revisar estados vazios:
   - sem projetos;
   - sem documento;
   - perguntas ainda nao geradas.
5. Revisar mensagens de erro vindas de ProblemDetails.
6. Garantir que erros tecnicos nao exponham stack trace ou detalhes sensiveis.
7. Garantir que a UI oriente o usuario conforme o status do projeto:
   - Draft;
   - QuestionsGenerated;
   - QuestionsAnswered;
   - DocumentGenerated.
8. Garantir que acoes incompativeis com o status nao fiquem disponiveis.
9. Garantir que o status seja exibido de forma amigavel.
10. Garantir que o status nao seja editavel.
11. Garantir que o frontend nao envie status em create/update.
12. Revisar navegacao:
   - voltar para projetos;
   - ir para documento;
   - logout;
   - redirecionamento de rota protegida.
13. Revisar responsividade basica para desktop e telas menores.
14. Manter visual simples, limpo e didatico.
15. Nao criar dashboard complexo.
16. Nao criar novas funcionalidades.

Tarefas tecnicas:
1. Remover duplicacoes simples no frontend, se houver.
2. Melhorar nomes de funcoes/componentes apenas se necessario.
3. Garantir que servicos de API continuam centralizados.
4. Garantir que tratamento de token continua centralizado.
5. Garantir que ProblemDetails continua tratado de forma consistente.
6. Nao alterar contratos da API.
7. Nao alterar backend, salvo bug critico identificado.
8. Nao adicionar dependencias novas sem necessidade clara.

Validacao:
1. Rodar em src/frontend/specpilot-web:
   - npm ci
   - npm run build
   - npm test

2. Rodar backend:
   - dotnet test src/backend/SpecPilot.sln

3. Rodar:
   - docker compose config

4. Se viavel:
   - docker compose down -v
   - docker compose up --build -d

5. Validar manualmente ou por HTTP/testes que o fluxo principal continua funcionando.

Documentacao:
1. Atualizar docs/development-log.md com uma entrada curta sobre o Prompt 18.
2. Atualizar docs/10-setup-guide.md somente se alguma instrucao real mudar.
3. Atualizar README.md somente se houver informacao incorreta sobre execucao.
4. Nao reescrever README storytelling nesta etapa.

Rastreabilidade:
1. Salve este proprio prompt em:
   - prompts/codex/18-polish-mvp-user-journey.md

2. Se o arquivo ja existir, atualize-o com esta versao.

Criterios de aceite:
- Fluxo principal continua funcionando.
- UI esta mais clara e didatica.
- Loading/error/success states estao consistentes.
- Acoes por status estao claras.
- Build frontend passa.
- Testes frontend passam.
- Testes backend passam.
- Docker Compose continua valido.
- Nenhuma funcionalidade fora do MVP foi adicionada.
- Backend nao foi alterado desnecessariamente.
- Documentacao de desenvolvimento foi atualizada.

Nao implemente nesta etapa:
- RAG;
- upload;
- PDF;
- chat livre;
- multiplos agentes;
- dashboard complexo;
- deploy;
- Playwright completo;
- README storytelling final.

Ao finalizar:
1. Rode validacoes aplicaveis.
2. Rode git status.
3. Liste arquivos alterados.
4. Faca commit usando Conventional Commits.

Mensagem de commit:
fix(frontend): polish mvp user journey
