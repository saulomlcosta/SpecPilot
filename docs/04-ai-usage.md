# 04 - Uso de IA Generativa

## Onde a IA sera aplicada

A IA sera usada em dois pontos do fluxo:

1. geracao de perguntas de refinamento
2. geracao do documento tecnico inicial

## Justificativa

Esses dois pontos concentram a maior incerteza sem exigir automacao excessiva. Assim, a IA apoia o raciocinio do usuario sem transformar o sistema em um chat generico.

## O que a IA nao fara

- nao escrevera codigo para o usuario final
- nao executara agentes autonomos multiplos
- nao acessara bases externas via RAG nesta etapa
- nao substituira validacoes de negocio deterministicas

## Estrategia de providers

O sistema deve oferecer:

- `FakeAiService` como padrao local e para testes
- provider OpenAI opcional via variavel de ambiente

## Abstracao adotada

A IA deve ficar isolada atras de uma interface de aplicacao, permitindo trocar o provider sem afetar casos de uso, controllers ou testes.

Nesta etapa:

- existe uma interface unica para operacoes de IA
- o provider `Fake` responde de forma fixa e previsivel
- a selecao do provider e feita por `Ai__Provider`
- o valor padrao local e em Docker deve ser `Fake`
- o provider `OpenAI` usa `HttpClient` encapsulado na Infrastructure
- `Ai__OpenAi__ApiKey` e `Ai__OpenAi__Model` controlam o provider real
- respostas da OpenAI devem ser validadas como JSON estruturado antes de seguir para a aplicacao

## Operacoes previstas

A abstracao de IA cobre dois comportamentos:

1. gerar perguntas de refinamento
2. gerar documento tecnico inicial

Essas operacoes seguem os prompts documentados em `prompts/runtime/`, mas nesta etapa o `FakeAiService` nao faz chamadas externas.

Quando `Ai__Provider=OpenAI`:

- os prompts runtime sao carregados de `prompts/runtime/`
- placeholders sao renderizados com os dados do projeto
- o provider solicita resposta em JSON
- a resposta bruta e validada antes de retornar para a aplicacao
- erros de rede, timeout, HTTP invalido e JSON invalido sao tratados de forma controlada

## Beneficios da estrategia

- reproducibilidade
- independencia de credenciais externas
- menor custo para avaliacao
- testes estaveis
