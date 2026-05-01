# ADR 0002 - Usar PostgreSQL

## Status

Aceita

## Contexto

O sistema precisa persistir usuarios, projetos, perguntas, respostas e documentos.

## Decisao

Usar PostgreSQL como banco de dados relacional principal.

## Alternativas consideradas

- SQLite
- bancos NoSQL orientados a documentos

## Justificativa

SQLite simplificaria ainda mais o setup, mas se distancia do cenario de aplicacao web previsto para as proximas etapas. Um banco NoSQL nao traz vantagem clara para o modelo relacional pequeno do MVP.

## Consequencias

- suporte robusto a integridade relacional
- boa compatibilidade com .NET
- execucao simples com Docker
