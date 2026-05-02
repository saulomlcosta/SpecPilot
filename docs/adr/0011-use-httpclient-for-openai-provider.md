# ADR 0011 - Usar HttpClient para o provider OpenAI

## Contexto

O projeto precisa oferecer um provider OpenAI opcional sem comprometer a execucao local com `FakeAiService`, a auditabilidade academica e a independencia de SDKs especificos nesta etapa.

## Decisao

Adotar `HttpClient` direto contra a API da OpenAI, encapsulado apenas na camada `Infrastructure`, usando `IHttpClientFactory` e mantendo `IAiService` como abstracao unica consumida pela `Application`.

## Motivacoes

- tornar o payload HTTP visivel e auditavel
- registrar prompt renderizado, resposta bruta, provider e modelo de forma clara
- reduzir acoplamento com SDK oficial
- facilitar testes sem rede com `HttpMessageHandler` fake
- preservar `FakeAiService` como padrao local e para testes

## Consequencias

- a infraestrutura passa a ser responsavel por renderizar prompts e validar JSON retornado
- erros de rede, timeout, HTTP invalido e JSON invalido precisam ser tratados explicitamente
- a selecao do provider continua configuravel por variavel de ambiente
- nenhum teste automatizado depende de chamada real para a OpenAI
