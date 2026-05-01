# 05 - Prompts

## Organizacao

Os prompts do projeto foram separados em dois grupos:

### `prompts/runtime/`

Prompts usados pela aplicacao em execucao para conversar com o provider de IA.

### `prompts/codex/`

Prompts usados para orientar o desenvolvimento assistido por IA ao longo do projeto.

## Padrao adotado para runtime

Os prompts de runtime seguirao o metodo **CO-STAR**:

- **C**ontext
- **O**bjective
- **S**tyle
- **T**one
- **A**udience
- **R**esponse

## Motivacao

Esse formato foi escolhido para:

- reduzir ambiguidade
- melhorar consistencia
- facilitar auditoria academica
- tornar o comportamento mais explicavel

