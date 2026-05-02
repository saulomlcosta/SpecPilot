# 08 - Estrategia de testes

## Objetivo

Garantir qualidade, seguranca de mudancas e execucao previsivel em ambiente local e academico.

## Tipos de teste previstos

### Testes unitarios

Devem cobrir:

- validacoes
- regras de negocio
- transformacoes de dados
- comportamentos do servico de IA fake
- selecao do provider de IA por configuracao
- protecao do fluxo de `ProjectStatus` nos casos de uso

### Testes de integracao

Devem cobrir:

- endpoints principais
- integracao com banco de dados
- fluxo principal do MVP de ponta a ponta:
  - registro
  - login
  - criacao de projeto
  - geracao de perguntas
  - resposta das perguntas
  - geracao de documento
  - consulta do documento gerado
- respostas `ProblemDetails` para erros esperados
- tratamento global de excecoes inesperadas

## Diretrizes

- cada caso de uso relevante deve ter cobertura proporcional
- testes nao devem depender de internet
- `FakeAiService` deve ser o padrao para testes
- o ambiente de integracao deve fixar explicitamente `Ai__Provider=Fake`
- cenarios felizes e invalidos devem ser cobertos
- cenarios de erro HTTP devem validar status e formato padronizado da resposta
- verificacoes de persistencia devem preferir consultas `AsNoTracking()` e chamadas assincronas
- os testes do backend tambem devem ser executados automaticamente no CI

## Aplicacao ao servico de IA

- a abstracao de IA deve ter testes unitarios proprios
- o `FakeAiService` deve retornar respostas deterministicas
- a selecao do provider deve poder ser verificada por configuracao
- nenhum teste desta etapa deve depender de credenciais externas
- nenhum teste desta etapa deve realizar chamada real a OpenAI

## Beneficio didatico

Essa estrategia ajuda a demonstrar separacao de responsabilidades e confiabilidade sem exigir infraestrutura externa complexa.

## Aplicacao no CI

O repositorio usa GitHub Actions para executar restore, build e testes do backend em cada `push` e `pull_request`.

Como os testes de integracao atuais usam banco em memoria com `WebApplicationFactory`, nao ha necessidade de configurar PostgreSQL manualmente no workflow nesta etapa. O CI fixa `Ai__Provider=Fake` para impedir chamadas reais a OpenAI.
