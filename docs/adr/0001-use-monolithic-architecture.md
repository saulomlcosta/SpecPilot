# ADR 0001 - Usar arquitetura monolitica modular

## Status

Aceita

## Contexto

O projeto possui escopo pequeno, equipe reduzida e foco didatico.

## Decisao

Adotar uma arquitetura monolitica modular com separacao clara entre camadas e responsabilidades.

## Alternativas consideradas

- microservicos
- monolito sem separacao modular explicita

## Justificativa

Microservicos adicionariam complexidade operacional e cognitiva desnecessaria para um MVP academico. Um monolito modular preserva simplicidade sem abrir mao de boas fronteiras internas.

## Consequencias

- menor complexidade operacional
- mais facilidade de entendimento
- evolucao adequada ao MVP
