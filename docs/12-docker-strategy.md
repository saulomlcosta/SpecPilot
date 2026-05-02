# 12 - Estrategia de Docker e Docker Compose

## Objetivo

Garantir reproducibilidade local, simplicidade de avaliacao e isolamento minimo entre dependencias.

## Estrategia adotada

- usar Docker Compose como ponto unico de subida do ambiente local
- subir PostgreSQL e API com `docker compose up --build`
- manter frontend como servico futuro e opcional
- manter configuracoes principais em variaveis de ambiente
- usar `Ai__Provider=Fake` por padrao

## Beneficios

- ambiente previsivel
- menor friccao para avaliacao
- reducao de diferencas entre maquinas
- independencia de chave externa de IA
- inicializacao automatica do schema necessario para o MVP

## Servicos desta etapa

### PostgreSQL

Responsavel pela persistencia relacional do projeto e exposto localmente na porta `5432`.

### API

Construida via Dockerfile proprio do backend e exposta localmente na porta `8080`.
A API cria automaticamente o schema do banco necessario para o MVP ao iniciar.

### Frontend futuro

O `docker-compose.yml` ja reserva um servico opcional para o frontend, mas esse servico nao participa da execucao padrao enquanto a aplicacao web ainda nao existe.

## Enderecos esperados

- API: `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`
- Health check: `http://localhost:8080/health`

## Papel do Dockerfile do backend

O Dockerfile do backend:

- restaura dependencias da API e das camadas compartilhadas
- publica a aplicacao em modo release
- gera uma imagem final menor baseada em `aspnet:8.0`

## Limites intencionais desta etapa

- nao ha frontend implementado
- nao ha dependencia obrigatoria de provider real de IA
