# 13 - Boas praticas de desenvolvimento

## Principios norteadores

- **SOLID:** orientar desenho de classes e servicos
- **DRY:** evitar duplicacao sem criar abstracoes prematuras
- **KISS:** priorizar simplicidade
- **YAGNI:** nao implementar o que o MVP nao exige
- **Clean Code:** nomes claros, metodos pequenos e responsabilidade bem definida

## Regras praticas

- validar dados de entrada nas bordas
- isolar regras de negocio
- manter infraestrutura substituivel
- documentar decisoes arquiteturais
- escrever testes desde cedo
- usar commits pequenos e descritivos

## Result Pattern neste projeto

Neste projeto, **Result Pattern** significa representar falhas esperadas de aplicacao com objetos de resultado em vez de usar exceptions para fluxo normal.

Exemplos de uso:

- `Result.Success()`
- `Result.Failure(error)`
- `Result.Success<T>(valor)`
- `Result.Failure<T>(error)`

## Por que usar Result Pattern

- evita uso de exceptions como fluxo esperado
- melhora previsibilidade dos handlers
- facilita testes unitarios
- separa melhor regras de negocio de detalhes HTTP
- torna o contrato de sucesso e falha mais explicito

## ProblemDetails neste projeto

`ProblemDetails` sera usado na camada Api para padronizar respostas HTTP de erro de forma legivel para clientes, avaliadores e futuras interfaces.

## Por que usar ProblemDetails

- padroniza respostas de erro HTTP
- melhora clareza para consumidores da API
- evita formatos ad hoc por endpoint
- mantem a camada Api responsavel por representar erros em HTTP

## Erro esperado x excecao inesperada

### Erro esperado de aplicacao

E uma situacao prevista pela regra de negocio ou pelo fluxo normal do sistema. Deve retornar `Result` ou `Result<T>`.

Exemplos:

- email ja cadastrado
- credenciais invalidas
- usuario nao encontrado
- acesso negado
- status invalido

### Excecao inesperada

E uma falha nao prevista como parte do fluxo normal. Deve continuar sendo tratada como exception.

Exemplos:

- falha de banco de dados
- erro nao tratado
- falha inesperada de infraestrutura

## Diretriz academica

Toda implementacao futura deve conseguir responder:

- qual problema resolve
- por que foi feita assim
- como pode ser testada
- qual decisao arquitetural a sustenta
