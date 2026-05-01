Estamos entre o Prompt 07 e o Prompt 08.

Antes de avancar para o Prompt 08, faca um ajuste controlado no modulo de Projects para proteger a regra de transicao de status do projeto.

Contexto:
O projeto SpecPilot AI usa o enum ProjectStatus para controlar o fluxo do MVP:

- Draft
- QuestionsGenerated
- QuestionsAnswered
- DocumentGenerated

O status nao deve ser alterado manualmente pelo endpoint comum de atualizacao de projeto.
O status deve mudar apenas pelos casos de uso especificos do fluxo:

- GenerateRefinementQuestions -> Draft para QuestionsGenerated
- AnswerRefinementQuestions -> QuestionsGenerated para QuestionsAnswered
- GenerateProjectDocument -> QuestionsAnswered para DocumentGenerated

Problema identificado:
O teste atual de update de projeto envia o campo status = "QuestionsGenerated" no payload e espera que o status seja alterado.
Isso abre a possibilidade de pular etapas do fluxo do MVP, o que viola a regra de negocio.

Objetivo:
Garantir que o endpoint PUT /api/projects/{id} atualize apenas dados editaveis do projeto, sem permitir alteracao manual de status.

Antes de alterar arquivos:
1. Leia AGENTS.md.
2. Leia docs/02-mvp-scope.md.
3. Leia docs/06-data-model.md.
4. Leia docs/07-api-contracts.md.
5. Leia docs/13-development-best-practices.md.
6. Leia docs/adr/0009-use-result-pattern-and-problemdetails.md, se existir.
7. Leia prompts/codex/05-implement-projects.md.
8. Leia prompts/codex/07-implement-refinement-questions.md.
9. Rode git status.
10. Rode git log --oneline -5.

Tarefas:
1. Ajustar o contrato/request de atualizacao de projeto para nao aceitar alteracao de Status.
   - O update comum deve aceitar apenas:
     - name
     - initialDescription
     - goal
     - targetAudience

2. Ajustar o handler/use case de update para nao alterar ProjectStatus.

3. Garantir que ProjectStatus seja alterado somente pelos casos de uso do fluxo:
   - geracao de perguntas;
   - resposta das perguntas;
   - geracao do documento.

4. Atualizar testes de integracao de Projects.

No arquivo de testes de endpoints de projetos:
- Altere o teste Update_should_change_owned_project para validar que apenas os campos editaveis sao alterados.
- O teste deve garantir que o Status continua Draft apos o update comum.

Exemplo esperado:
- Criar projeto -> Status Draft
- Atualizar name, initialDescription, goal e targetAudience
- Resposta HTTP 200
- Campos editaveis atualizados
- Status permanece Draft

5. Adicionar um teste especifico:

Update_should_not_allow_manual_status_change

Esse teste deve:
- Criar um projeto com Status Draft.
- Enviar um payload de update contendo status = "QuestionsGenerated".
- Garantir que o status nao seja alterado manualmente.

Escolha uma das abordagens abaixo e documente a decisao:
A) Ignorar o campo status se ele vier no payload.
B) Retornar 400 Bad Request se o contrato detectar tentativa de alterar status.

Preferencia para este MVP:
- Usar a abordagem A, mantendo o contrato de update sem a propriedade Status.
- Se o JSON vier com status extra, ele deve ser ignorado pelo model binding.
- O status deve permanecer Draft.

6. Adicionar testes de seguranca/autorizacao se ainda nao existirem:
- Update_should_return_not_found_for_project_from_another_user
- Delete_should_return_not_found_for_project_from_another_user
- Get_by_id_should_return_not_found_for_unknown_project

7. Se o projeto usa Result Pattern + ProblemDetails:
- Falhas esperadas devem retornar Result.Failure.
- Projeto inexistente deve retornar NotFound.
- Projeto de outro usuario deve retornar NotFound para evitar vazamento de informacao.
- Nao usar exceptions para fluxo esperado.

8. Atualizar docs/07-api-contracts.md explicando que o endpoint PUT /api/projects/{id} nao aceita alteracao de status.

9. Atualizar docs/06-data-model.md explicando que ProjectStatus e controlado pelos casos de uso do fluxo, nao por update manual.

10. Atualizar docs/13-development-best-practices.md se necessario, citando protecao de invariantes de fluxo.

11. Criar uma ADR:

docs/adr/0010-protect-project-status-transitions.md

Conteudo esperado da ADR:

# ADR 0010: Proteger transicoes de status do projeto

## Status
Aceita

## Contexto
O SpecPilot AI usa ProjectStatus para controlar as etapas do fluxo principal do MVP. Permitir que o endpoint comum de update altere o status permitiria pular etapas obrigatorias e quebrar regras de negocio.

## Decisao
O endpoint de atualizacao de projeto nao podera alterar ProjectStatus. O status sera alterado apenas por casos de uso especificos do fluxo: geracao de perguntas, resposta das perguntas e geracao de documento.

## Consequencias positivas
- Protege o fluxo principal do MVP.
- Evita estados invalidos.
- Mantem regras de negocio centralizadas nos casos de uso corretos.
- Melhora previsibilidade dos testes.

## Consequencias negativas
- Mudancas de status exigem casos de uso especificos.
- Pode exigir mais codigo quando novos status forem adicionados.

## Alternativas consideradas
- Permitir alteracao manual de status no update.
- Validar status no update comum.
- Criar endpoint administrativo para status.

12. Criar o arquivo de rastreabilidade do prompt:

prompts/codex/07a-protect-project-status-transitions.md

Salve este proprio prompt nesse arquivo.

13. Atualizar README.md somente se houver uma secao de decisoes ou links para ADRs que precise incluir a nova ADR.

14. Rodar testes aplicaveis:
- testes de Projects;
- testes de GenerateRefinementQuestions;
- dotnet test, se viavel.

15. Rode git status ao final.

16. Fazer commit usando Conventional Commits.

Mensagem de commit:
fix(projects): protect project status transitions

Regras:
- Nao implemente o Prompt 08 ainda.
- Nao implemente geracao de documento.
- Nao implemente frontend.
- Nao adicione funcionalidades fora do MVP.
- Mantenha o ajuste pequeno e focado.
