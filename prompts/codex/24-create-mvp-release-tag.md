Crie a tag e a release do MVP 1.0.0 do SpecPilot AI mantendo o padrao de rastreabilidade do projeto.

Contexto:
A verificacao final de entrega foi concluida no commit:
0257a8f chore: final delivery check

O projeto esta pronto para entrega do MVP:
- README storytelling final concluido;
- checklist de avaliacao criado;
- release notes criadas;
- backend testado;
- frontend testado;
- Docker Compose validado;
- CI backend + frontend configurado;
- prompts versionados;
- ADRs e development log atualizados.

Objetivo:
Criar uma tag Git anotada v1.0.0 e, se possivel, criar uma GitHub Release baseada em docs/15-release-notes.md.

Antes de comecar:
1. Leia:
   - README.md
   - docs/15-release-notes.md
   - docs/development-log.md
   - prompts/codex/23-final-delivery-check.md

2. Rode:
   - git status
   - git log --oneline -10
   - git tag --list

Validacoes antes da tag:
1. Confirme que o working tree esta limpo.
2. Confirme que a branch atual e master.
3. Confirme que o commit final desejado esta no HEAD.
4. Confirme que o commit final inclui:
   - README final;
   - checklist;
   - release notes;
   - final delivery check.
5. Confirme que nao existe tag v1.0.0.
6. Confirme se a branch local esta ahead da origin.
7. Se estiver ahead, faca push da branch antes de criar/enviar a tag:
   - git push origin master

Rastreabilidade:
1. Crie o arquivo:
   - prompts/codex/24-create-mvp-release-tag.md

2. Salve este proprio prompt nesse arquivo.

3. Atualize docs/development-log.md com uma entrada curta sobre a criacao da tag/release MVP 1.0.0.

A entrada deve explicar:
- que v1.0.0 marca a versao final do MVP;
- que a tag aponta para o estado apos o final delivery check;
- que a release usa docs/15-release-notes.md como base;
- que nenhuma funcionalidade foi alterada nesta etapa.

Commit documental antes da tag:
1. Se docs/development-log.md e prompts/codex/24-create-mvp-release-tag.md foram alterados, faca commit:
   docs: record mvp release tag process

2. Depois rode:
   - git status
   - git log --oneline -5

3. Faca push da branch:
   - git push origin master

Criacao da tag:
1. Crie uma tag anotada:
   git tag -a v1.0.0 -m "SpecPilot AI MVP 1.0.0"

2. Valide:
   git show v1.0.0 --no-patch
   git tag --list

3. Envie a tag:
   git push origin v1.0.0

Release no GitHub:
1. Se GitHub CLI estiver disponivel e autenticado, crie uma release:

   gh release create v1.0.0 --title "SpecPilot AI MVP 1.0.0" --notes-file docs/15-release-notes.md

2. Se GitHub CLI nao estiver disponivel ou nao estiver autenticado:
   - nao falhe a etapa;
   - registre que a tag foi enviada;
   - informe que a release deve ser criada manualmente no GitHub usando docs/15-release-notes.md.

Validacao final:
1. Rode:
   - git status
   - git tag --list
   - git show v1.0.0 --no-patch
   - git ls-remote --tags origin v1.0.0

Criterios de aceite:
- prompts/codex/24-create-mvp-release-tag.md criado.
- docs/development-log.md atualizado.
- commit documental criado, se houve alteracao.
- branch master enviada ao GitHub.
- tag anotada v1.0.0 criada.
- tag v1.0.0 enviada ao GitHub.
- release criada via GitHub CLI, se possivel.
- se release nao for criada via CLI, orientacao manual registrada.
- nenhum codigo funcional alterado.
- working tree final limpo.

Nao faca:
- nao criar tag se houver alteracoes nao commitadas;
- nao usar tag lightweight;
- nao usar git push --force;
- nao alterar backend;
- nao alterar frontend;
- nao alterar Docker;
- nao alterar workflow;
- nao criar deploy;
- nao publicar Docker image.
