# 12 - Estrategia de Docker e Docker Compose

## Objetivo

Garantir reproducibilidade local, simplicidade de avaliacao e isolamento minimo entre dependencias.

## Estrategia adotada

- usar Docker Compose como ponto unico de subida do ambiente local
- iniciar pelo PostgreSQL nesta etapa
- adicionar backend e frontend ao compose somente quando existirem de fato
- manter configuracoes principais em variaveis de ambiente

## Beneficios

- ambiente previsivel
- menor friccao para avaliacao
- reducao de diferencas entre maquinas

## Papel do `docker-compose.yml` nesta fase

O arquivo atual provisiona apenas:

- banco PostgreSQL
- volume persistente
- healthcheck

Isso atende a necessidade de preparar a base sem implementar aplicacao antes da hora.

