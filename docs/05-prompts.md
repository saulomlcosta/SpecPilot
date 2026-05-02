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

Os prompts de `prompts/codex/` registram a evolucao do desenvolvimento assistido por IA em etapas pequenas e versionadas.

O conjunto atual cobre, entre outros pontos:

- criacao e revisao da base documental;
- estrutura inicial de backend;
- Docker e Docker Compose;
- autenticacao;
- gerenciamento de projetos;
- abstracao de IA, `FakeAiService` e provider OpenAI opcional;
- geracao de perguntas e documento;
- testes backend e frontend;
- CI com GitHub Actions;
- validacao do Docker Compose;
- polimento da jornada do usuario;
- checklist de avaliacao;
- README final em formato storytelling.

Isso permite rastrear o processo de desenvolvimento do projeto de ponta a ponta, sem depender apenas do historico da conversa.

## Criterios de qualidade para prompts

- aderencia explicita ao escopo do MVP
- saida esperada clara
- restricoes objetivas
- linguagem didatica e auditavel
