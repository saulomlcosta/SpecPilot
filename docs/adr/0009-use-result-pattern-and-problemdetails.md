# ADR 0009: Usar Result Pattern e ProblemDetails

## Status
Aceita

## Contexto
O projeto precisa tratar erros esperados de aplicacao sem usar exceptions como fluxo de controle, alem de retornar respostas HTTP padronizadas e compreensiveis para clientes da API.

## Decisao
Usar Result Pattern para falhas esperadas de aplicacao e ProblemDetails para representar erros HTTP na camada Api.

## Consequencias positivas
- Evita uso de exceptions como fluxo esperado.
- Melhora testabilidade dos handlers.
- Padroniza respostas de erro.
- Separa erros esperados de falhas inesperadas.
- Mantem Domain/Application desacoplados de HTTP.

## Consequencias negativas
- Exige mapeamento explicito entre Result e respostas HTTP.
- Pode adicionar um pouco mais de codigo estrutural.

## Alternativas consideradas
- Usar exceptions para todos os erros.
- Retornar respostas HTTP diretamente dos handlers.
- Criar formato proprio de erro.
