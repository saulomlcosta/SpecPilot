# ADR 0006 - Usar Docker Compose

## Status

Aceita

## Contexto

O ambiente precisa ser simples de subir por avaliadores e desenvolvedores.

## Decisao

Usar Docker Compose como orquestrador local do ambiente.

## Alternativas consideradas

- setup manual sem containers
- scripts locais especificos por sistema operacional

## Justificativa

Docker Compose reduz variacoes de ambiente e facilita a avaliacao por terceiros, especialmente quando o objetivo e demonstrar reproducibilidade com pouco atrito.

## Consequencias

- setup mais facil
- reproducibilidade local
- menor dependencia de configuracao manual
