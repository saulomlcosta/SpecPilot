# 07 - Contratos iniciais de API

## Objetivo

Este documento descreve contratos iniciais para orientar a futura implementacao da Web API. Nao se trata de especificacao final, mas de uma base de alinhamento.

## Endpoints previstos

### Autenticacao

- `POST /api/auth/register`
- `POST /api/auth/login`

### Projetos

- `POST /api/projects`
- `GET /api/projects`
- `GET /api/projects/{id}`

### Refinamento

- `POST /api/projects/{id}/refinement-questions`
- `POST /api/projects/{id}/refinement-answers`

### Documento

- `POST /api/projects/{id}/initial-document`
- `GET /api/projects/{id}/initial-document`

## Exemplo conceitual de criacao de projeto

```json
{
  "name": "Sistema de clinica",
  "initialDescription": "Quero um sistema para agendamento, prontuario e notificacoes."
}
```

## Exemplo conceitual de documento gerado

```json
{
  "overview": "Sistema web para apoiar a operacao de uma clinica.",
  "functionalRequirements": [
    "Permitir cadastro de pacientes",
    "Permitir agendamento de consultas"
  ],
  "nonFunctionalRequirements": [
    "Disponibilidade em horario comercial",
    "Controle basico de acesso"
  ],
  "useCases": [
    "Cadastrar paciente",
    "Agendar consulta"
  ],
  "risks": [
    "Descricao inicial insuficiente",
    "Requisitos ambiguos"
  ]
}
```

