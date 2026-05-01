# ADR 0008 - Usar FakeAiService para testes e execucao local

## Status

Aceita

## Contexto

Dependencia obrigatoria de provider externo encarece e dificulta testes, demonstracoes e avaliacao.

## Decisao

Implementar `FakeAiService` como comportamento padrao para desenvolvimento local e testes, mantendo OpenAI como opcional via variavel de ambiente.

## Alternativas consideradas

- depender exclusivamente de provider real
- mockar IA apenas em testes unitarios isolados

## Justificativa

O projeto precisa demonstrar um fluxo funcional mesmo sem credenciais externas. Um fake controlado permite testes, demonstracoes e desenvolvimento local com menor custo e maior previsibilidade.

## Consequencias

- ambiente mais previsivel
- menor custo
- independencia de credenciais externas
