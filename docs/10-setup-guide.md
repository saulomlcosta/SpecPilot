# 10 - Guia de setup

## Objetivo

Permitir que um avaliador prepare e execute o ambiente local da maneira mais simples possivel.

## Pre-requisitos

- Git
- Docker
- Docker Compose

## Passos desta etapa

1. clonar o repositorio
2. copiar `.env.example` para `.env`
3. subir os containers com Docker Compose

## Comandos

No Linux ou macOS:

```bash
cp .env.example .env
docker compose up --build
```

No PowerShell:

```powershell
Copy-Item .env.example .env
docker compose up --build
```

## Variavel de ambiente relevante

- `Cors__AllowedOrigins=http://localhost:3000;http://127.0.0.1:3000`

Essa configuracao permite que o frontend local acesse a API no ambiente de desenvolvimento e no Docker Compose sem abrir CORS para qualquer origem.

## Resultado esperado nesta fase

- um container PostgreSQL em execucao
- um container da API em execucao
- um container do frontend em execucao
- frontend acessivel em `http://localhost:3000`
- Swagger acessivel em `http://localhost:8080/swagger`
- health check acessivel em `http://localhost:8080/health`
- schema inicial do banco criado automaticamente pela API no startup

## Observacao importante

O frontend desta etapa ja cobre autenticacao, gerenciamento de projetos e fluxo principal de IA do MVP (gerar perguntas, responder perguntas, gerar documento e visualizar documento).
