Implemente a fatia de autenticacao do SpecPilot AI.

Antes de comecar, leia:

- AGENTS.md
- docs/02-mvp-scope.md
- docs/07-api-contracts.md
- docs/08-testing-strategy.md
- docs/09-security.md
- docs/13-development-best-practices.md

Objetivo:
Permitir cadastro, login e identificacao do usuario autenticado.

Funcionalidades:
- Cadastro de usuario
- Login
- Endpoint para retornar o usuario autenticado
- Geracao de JWT
- Hash de senha

Endpoints:
- POST /api/auth/register
- POST /api/auth/login
- GET /api/auth/me

Regras:
1. Nao colocar logica de negocio nos controllers.
2. Usar MediatR para os casos de uso.
3. Usar FluentValidation para validacoes.
4. Usar hash seguro para senha.
5. Nao retornar PasswordHash em nenhuma resposta.
6. Validar email obrigatorio e formato de email.
7. Validar senha minima.
8. Nao permitir cadastro com email duplicado.
9. Retornar erros de validacao de forma padronizada.
10. Criar testes unitarios para handlers e validators.
11. Criar testes de integracao basicos para os endpoints.
12. Atualizar documentacao de API se necessario.
13. Salvar este proprio prompt em:
    - prompts/codex/04-implement-auth.md
14. Fazer commit ao final usando Conventional Commits.

## Ajuste durante a execucao: Result Pattern + ProblemDetails

Durante a implementacao de Auth, foi decidido adicionar Result Pattern para falhas esperadas de aplicacao e ProblemDetails para respostas HTTP padronizadas.

Essa decisao foi aplicada para:

- evitar exceptions como fluxo esperado de negocio
- padronizar erros HTTP na camada Api
- manter Domain e Application desacoplados de ASP.NET Core

Criterios de aceite:
- Um usuario consegue se cadastrar.
- Um usuario consegue fazer login.
- O login retorna um token JWT.
- O endpoint /api/auth/me retorna dados do usuario autenticado.
- Requisicoes sem token para /api/auth/me retornam 401.
- Os testes unitarios e de integracao passam.

Nao implemente:
- Recuperacao de senha
- Confirmacao de email
- OAuth
- Refresh token
- Roles avancadas
- Frontend

Mensagem de commit:
feat(auth): implement authentication flow
