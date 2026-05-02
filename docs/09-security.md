# 09 - Seguranca

## Medidas adotadas e reforcadas no MVP

- armazenamento seguro de senha com hash adequado
- validacao de entrada em todas as bordas da API
- controle de acesso aos projetos por usuario autenticado
- uso de variaveis de ambiente para configuracoes sensiveis
- nao exposicao de chaves externas no codigo-fonte

## Estado atual desta etapa

O backend e o frontend do MVP ja foram implementados. Este documento resume os cuidados de seguranca coerentes com o estado atual do projeto, sem transformar o MVP em uma solucao de seguranca avancada fora de escopo.

Controles visiveis nesta etapa:

- autenticacao com JWT;
- rotas protegidas no backend e no frontend;
- isolamento de projetos por usuario autenticado;
- `ProjectStatus` protegido contra alteracao manual no fluxo comum;
- respostas HTTP de erro padronizadas sem stack trace publico;
- chave OpenAI mantida fora do frontend e fora do codigo versionado;
- `FakeAiService` como padrao para execucao local e testes sem dependencia externa.

## Cuidados com IA

- nao confiar cegamente no texto retornado pelo provider
- validar formato de saida
- tratar respostas de IA como dado externo

## Cuidados com erros e logs

- respostas HTTP de erro nao devem expor stack trace
- mensagens publicas devem permanecer seguras e genericas para falhas inesperadas
- logs devem evitar senha, token JWT, API key e outros segredos
- quando possivel, diagnostico deve usar IDs tecnicos em vez de dados sensiveis
