# Prompt 27 - Criar patch release de CORS

Recupere o contexto pelo repositório antes de continuar.

Contexto:
O SpecPilot AI já teve uma release/tag v1.0.0 criada para o MVP.
Após a v1.0.0, foi corrigido um bug real de CORS entre:
- frontend: http://localhost:3000
- backend: http://localhost:8080

Essa correção permite que o navegador faça chamadas do frontend para a API durante login/cadastro e demais fluxos.

Objetivo:
Criar uma patch release v1.0.1 para registrar a correção de CORS, mantendo o padrão de rastreabilidade do projeto:
- prompt versionado em prompts/codex;
- development log atualizado;
- release notes atualizadas;
- tag anotada;
- GitHub Release, se possível.

Antes de começar, leia:
- README.md
- docs/15-release-notes.md
- docs/development-log.md
- prompts/codex/25-fix-cors-for-frontend-auth.md, se existir
- .github/workflows/ci.yml

Depois rode:
- git status
- git log --oneline -10
- git tag --list

Validações antes da patch release:
1. Confirme que o commit de correção de CORS existe no histórico local.
2. Confirme que o working tree está limpo.
3. Confirme que a branch atual é master.
4. Confirme que a tag v1.0.0 já existe.
5. Confirme que a tag v1.0.1 ainda não existe.
6. Confirme que a branch master está atualizada com o remoto ou faça push antes da tag, se necessário.

Documentação:
1. Atualize docs/15-release-notes.md adicionando uma seção para a versão patch:

## MVP 1.0.1

### Tipo
Patch release

### Resumo
Correção da configuração de CORS para permitir chamadas do frontend local em http://localhost:3000 para a API em http://localhost:8080.

### Correções
- Configuração de CORS no backend ASP.NET Core.
- Permissão explícita para as origens locais do frontend.
- Suporte ao header Authorization nas chamadas do frontend.
- Validação do fluxo de login/cadastro pelo navegador.

### Impacto
- A versão v1.0.1 é a versão recomendada para avaliação.
- A v1.0.0 permanece no histórico como a primeira release do MVP.
- Nenhuma funcionalidade nova foi adicionada.
- O escopo do MVP não foi alterado.

2. Atualize docs/development-log.md com uma entrada curta explicando:
- que a v1.0.1 foi criada após a correção de CORS;
- que a tag v1.0.0 continua apontando para o estado anterior;
- que v1.0.1 é a versão recomendada para avaliação;
- que não houve nova funcionalidade, apenas correção de integração browser/frontend/backend.

3. Atualize README.md apenas se necessário para indicar:
- versão recomendada para avaliação: v1.0.1

Não reescreva o README inteiro.

Rastreabilidade:
1. Crie o arquivo:
   - prompts/codex/27-create-cors-patch-release.md

2. Salve este próprio prompt nesse arquivo.

Commit documental antes da tag:
1. Se docs/15-release-notes.md, docs/development-log.md, README.md ou prompts/codex/27-create-cors-patch-release.md forem alterados, faça commit:

docs: record cors patch release

2. Depois rode:
- git status
- git log --oneline -5

3. Faça push da branch:
- git push origin master

Criação da tag:
1. Crie uma tag anotada:

git tag -a v1.0.1 -m "SpecPilot AI MVP 1.0.1 - CORS fix"

2. Valide a tag:

git show v1.0.1 --no-patch
git tag --list

3. Envie a tag:

git push origin v1.0.1

GitHub Release:
1. Se GitHub CLI estiver disponível e autenticado, crie a release:

gh release create v1.0.1 --title "SpecPilot AI MVP 1.0.1" --notes-file docs/15-release-notes.md

2. Se preferir criar manualmente, não falhe a etapa. Apenas registre que:
- a tag v1.0.1 foi enviada;
- a release deve ser criada manualmente no GitHub;
- a descrição pode usar a seção MVP 1.0.1 de docs/15-release-notes.md.

Validação final:
1. Rode:
- git status
- git tag --list
- git show v1.0.1 --no-patch
- git ls-remote --tags origin v1.0.1

Critérios de aceite:
- docs/15-release-notes.md possui seção v1.0.1.
- docs/development-log.md registra a patch release.
- prompts/codex/27-create-cors-patch-release.md existe.
- README menciona v1.0.1 como versão recomendada, se necessário.
- Commit documental criado.
- Branch master enviada ao remoto.
- Tag anotada v1.0.1 criada.
- Tag v1.0.1 enviada ao GitHub.
- GitHub Release v1.0.1 criada, se possível.
- Nenhum código funcional alterado nesta etapa.
- Working tree final limpo.

Não faça:
- Não mover a tag v1.0.0.
- Não usar git push --force.
- Não apagar release/tag antiga.
- Não criar tag lightweight.
- Não implementar funcionalidade.
- Não alterar backend.
- Não alterar frontend.
- Não alterar Docker.
- Não alterar workflow.
