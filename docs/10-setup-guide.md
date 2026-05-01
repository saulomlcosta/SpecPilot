# 10 - Guia de setup

## Objetivo

Permitir que um avaliador prepare o ambiente local da maneira mais simples possivel.

## Pre-requisitos

- Git
- Docker
- Docker Compose

## Passos desta etapa

1. clonar o repositorio
2. copiar `.env.example` para `.env`
3. subir os containers com Docker Compose

## Comandos

```bash
cp .env.example .env
docker compose up -d
docker compose ps
```

## Observacao importante

Nesta fase, apenas o PostgreSQL e provisionado. Backend e frontend serao adicionados nas proximas etapas do projeto.

