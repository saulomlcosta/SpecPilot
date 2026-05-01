# ADR 0003 - Usar saida estruturada da IA

## Status

Aceita

## Contexto

Saidas livres aumentam ambiguidade e dificultam testes.

## Decisao

Preferir respostas estruturadas da IA para perguntas de refinamento e documento inicial.

## Alternativas consideradas

- resposta totalmente livre em texto
- pos-processamento de texto nao estruturado

## Justificativa

Como o projeto precisa ser testavel e didatico, a estrutura explicita da resposta facilita validacao, parsing e comparacao de resultados.

## Consequencias

- parsing mais previsivel
- menor risco de inconsistencias
- melhor testabilidade
