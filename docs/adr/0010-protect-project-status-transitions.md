# ADR 0010: Proteger transicoes de status do projeto

## Status
Aceita

## Contexto
O SpecPilot AI usa ProjectStatus para controlar as etapas do fluxo principal do MVP. Permitir que o endpoint comum de update altere o status permitiria pular etapas obrigatorias e quebrar regras de negocio.

## Decisao
O endpoint de atualizacao de projeto nao podera alterar ProjectStatus. O status sera alterado apenas por casos de uso especificos do fluxo: geracao de perguntas, resposta das perguntas e geracao de documento.

## Consequencias positivas
- Protege o fluxo principal do MVP.
- Evita estados invalidos.
- Mantem regras de negocio centralizadas nos casos de uso corretos.
- Melhora previsibilidade dos testes.

## Consequencias negativas
- Mudancas de status exigem casos de uso especificos.
- Pode exigir mais codigo quando novos status forem adicionados.

## Alternativas consideradas
- Permitir alteracao manual de status no update.
- Validar status no update comum.
- Criar endpoint administrativo para status.
