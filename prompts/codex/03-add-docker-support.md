# Prompt Codex - Adicionar suporte completo a Docker

Objetivo:
Expandir a estrategia atual de Docker para incluir backend e frontend quando esses componentes existirem, mantendo simplicidade e foco didatico.

Diretrizes:

- preservar `docker-compose.yml` como ponto unico de execucao local
- criar Dockerfiles separados por aplicacao quando necessario
- manter uso de variaveis de ambiente
- evitar complexidade desnecessaria
- garantir compatibilidade com `FakeAiService`

Resultado esperado:

- ambiente local reproduzivel
- servicos integrados via Docker Compose
- instrucoes atualizadas no README
