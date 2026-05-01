# AGENTS.md

## Objetivo deste arquivo

Este documento define regras operacionais para agentes de IA que atuarem no projeto **SpecPilot AI**, com foco em previsibilidade, qualidade e aderencia ao escopo academico.

## Regras gerais

- escrever toda a documentacao, comentarios e artefatos principais em portugues
- respeitar estritamente o escopo do MVP
- nao implementar funcionalidades fora do MVP
- preferir solucoes simples, legiveis e didaticas
- registrar decisoes importantes em ADRs
- manter prompts versionados em arquivos dedicados
- usar Conventional Commits

## Escopo permitido nesta fase inicial

- estruturar documentacao
- definir arquitetura proposta
- documentar fluxos, modelo de dados e contratos
- preparar ambiente com Docker e Docker Compose
- registrar estrategia de testes

## Escopo proibido nesta fase inicial

- implementar backend
- implementar frontend
- adicionar mensageria
- adicionar microservicos
- adicionar RAG
- integrar servicos externos obrigatorios
- criar recursos fora do MVP aprovado

## Diretrizes de engenharia

- aplicar SOLID quando fizer sentido
- evitar duplicacao desnecessaria
- preferir KISS sobre sofisticacao prematura
- aplicar YAGNI com rigor academico
- separar responsabilidades por camada e por caso de uso
- validar entradas nas bordas da aplicacao
- manter regras de negocio fora de detalhes de infraestrutura
- priorizar nomes claros e intencao explicita

## Diretrizes para IA

- usar `FakeAiService` como comportamento padrao para desenvolvimento e testes
- tratar provider OpenAI como opcional via variavel de ambiente
- documentar prompts com contexto, objetivo, formato esperado e restricoes
- preferir saidas estruturadas para reduzir ambiguidade
- registrar claramente onde a IA agrega valor e onde nao deve ser usada

## Diretrizes para testes

- todo comportamento relevante deve ser testavel
- priorizar testes unitarios para regras de negocio
- usar testes de integracao para fluxos da API e persistencia
- evitar dependencia obrigatoria de internet para testes
- nao depender de chave externa para pipeline local

## Estrutura de pastas esperada

```text
docs/
prompts/
src/
tests/
```

## Processo recomendado para proximas etapas

1. revisar documentacao e ADRs antes de implementar
2. criar esqueleto do backend em .NET 8
3. adicionar testes unitarios desde o inicio
4. integrar PostgreSQL via Docker Compose
5. implementar `FakeAiService`
6. adicionar provider OpenAI opcional
7. criar frontend React com fluxo minimo do MVP
8. validar fluxo ponta a ponta

## Criterios de aceite para futuras entregas

- aderencia ao MVP
- documentacao atualizada
- testes automatizados relevantes
- ambiente local reproduzivel com Docker
- commit com convencao correta

