# 12 - Estrategia de Docker e Docker Compose

## Objetivo

Garantir reproducibilidade local, simplicidade de avaliacao e isolamento minimo entre dependencias.

## Estrategia adotada

- usar Docker Compose como ponto unico de subida do ambiente local
- subir PostgreSQL, API e frontend com `docker compose up --build`
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

### Frontend

O frontend React desta etapa roda na porta `3000`, usando `VITE_API_BASE_URL` apontando por padrao para `http://localhost:8080`.
O fluxo funcional atual do MVP ja permite:

- cadastro e login
- criacao e gerenciamento de projetos
- geracao de perguntas de refinamento
- resposta das perguntas com `questionId` real
- geracao do documento tecnico inicial
- visualizacao do documento gerado

## Enderecos esperados

- API: `http://localhost:8080`
- Frontend: `http://localhost:3000`
- Swagger: `http://localhost:8080/swagger`
- Health check: `http://localhost:8080/health`

## Papel do Dockerfile do backend

O Dockerfile do backend:

- restaura dependencias da API e das camadas compartilhadas
- publica a aplicacao em modo release
- gera uma imagem final menor baseada em `aspnet:8.0`

## Limites intencionais desta etapa

- nao ha dependencia obrigatoria de provider real de IA
- o provider OpenAI continua opcional e nao e necessario para avaliacao local
