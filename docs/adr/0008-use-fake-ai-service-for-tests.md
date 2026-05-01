# ADR 0008 - Usar FakeAiService para testes e execucao local

## Status

Aceita

## Contexto

Dependencia obrigatoria de provider externo encarece e dificulta testes, demonstracoes e avaliacao.

## Decisao

Implementar `FakeAiService` como comportamento padrao para desenvolvimento local e testes, mantendo OpenAI como opcional via variavel de ambiente.

## Consequencias

- ambiente mais previsivel
- menor custo
- independencia de credenciais externas

