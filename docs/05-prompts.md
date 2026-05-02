# 05 - Prompts

## Organizacao

Os prompts do projeto foram separados em dois grupos:

### `prompts/runtime/`

Prompts usados pela aplicacao em execucao para conversar com o provider de IA.

### `prompts/codex/`

Prompts usados para orientar o desenvolvimento assistido por IA ao longo do projeto.

## Prompts de runtime existentes

- `generate-refinement-questions.costar.md`
  Define como a IA deve gerar perguntas de refinamento a partir da descricao inicial.
- `generate-project-document.costar.md`
  Define como a IA deve consolidar descricao inicial e respostas em um documento tecnico inicial.

## Padrao adotado para runtime

Os prompts de runtime seguem o metodo **CO-STAR**:

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

## Renderizacao em runtime

Quando o provider OpenAI estiver habilitado:

- os arquivos de `prompts/runtime/` sao lidos pela Infrastructure
- placeholders como `{{ProjectName}}`, `{{InitialDescription}}`, `{{Goal}}`, `{{TargetAudience}}` e `{{RefinementAnswers}}` sao substituidos antes da chamada HTTP
- o prompt renderizado e registrado para auditoria no log de interacao com IA
- a resposta solicitada continua sendo JSON estruturado

## Prompts do Codex existentes

- `00-create-documentation-base.md`
  Registra a solicitacao original para criar a base documental do projeto.
- `01-review-documentation.md`
  Registra a etapa de revisao documental e validacao de consistencia.
- `02-create-backend-skeleton.md`
  Define a futura etapa de criacao do esqueleto do backend.
- `03-add-docker-support.md`
  Define a futura expansao da estrategia de Docker quando houver aplicacoes implementadas.

## Criterios de qualidade para prompts

- aderencia explicita ao escopo do MVP
- saida esperada clara
- restricoes objetivas
- linguagem didatica e auditavel
