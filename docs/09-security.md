# 09 - Seguranca

## Medidas previstas para o MVP

- armazenamento seguro de senha com hash adequado
- validacao de entrada em todas as bordas da API
- controle de acesso aos projetos por usuario autenticado
- uso de variaveis de ambiente para configuracoes sensiveis
- nao exposicao de chaves externas no codigo-fonte

## Limites desta etapa

Como backend e frontend ainda nao foram implementados, este documento registra apenas diretrizes iniciais de seguranca.

## Cuidados com IA

- nao confiar cegamente no texto retornado pelo provider
- validar formato de saida
- tratar respostas de IA como dado externo

## Cuidados com erros e logs

- respostas HTTP de erro nao devem expor stack trace
- mensagens publicas devem permanecer seguras e genericas para falhas inesperadas
- logs devem evitar senha, token JWT, API key e outros segredos
- quando possivel, diagnostico deve usar IDs tecnicos em vez de dados sensiveis
