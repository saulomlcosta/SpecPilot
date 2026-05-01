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

### Testes de integracao

Devem cobrir:

- endpoints principais
- integracao com banco de dados
- fluxo de criacao de projeto
- fluxo de geracao de perguntas
- fluxo de geracao de documento

## Diretrizes

- cada caso de uso relevante deve ter cobertura proporcional
- testes nao devem depender de internet
- `FakeAiService` deve ser o padrao para testes
- cenarios felizes e invalidos devem ser cobertos

## Aplicacao ao servico de IA

- a abstracao de IA deve ter testes unitarios proprios
- o `FakeAiService` deve retornar respostas deterministicas
- a selecao do provider deve poder ser verificada por configuracao
- nenhum teste desta etapa deve depender de credenciais externas

## Beneficio didatico

Essa estrategia ajuda a demonstrar separacao de responsabilidades e confiabilidade sem exigir infraestrutura externa complexa.
