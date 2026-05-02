using SpecPilot.Application.Abstractions.Ai;
using SpecPilot.Application.Ai.Models;

namespace SpecPilot.Infrastructure.Ai;

public class FakeAiService : IAiService
{
    public Task<GenerateRefinementQuestionsResponse> GenerateRefinementQuestionsAsync(
        GenerateRefinementQuestionsRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = new GenerateRefinementQuestionsResponse
        {
            Questions =
            [
                "Quem sao os principais usuarios do sistema?",
                "Quais funcionalidades sao essenciais para a primeira versao?",
                "Existe alguma regra de negocio que precisa ser respeitada desde o inicio?",
                "Quais informacoes o usuario precisara informar para usar o sistema?",
                "Ha alguma restricao tecnica, operacional ou de prazo importante para o MVP?"
            ],
            Metadata = new AiResponseMetadata
            {
                Provider = "Fake",
                Model = "fake-static-response"
            }
        };

        return Task.FromResult(response);
    }

    public Task<GenerateProjectDocumentResponse> GenerateProjectDocumentAsync(
        GenerateProjectDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = new GenerateProjectDocumentResponse
        {
            Overview = "Documento inicial gerado pelo FakeAiService para apoiar desenvolvimento local e testes automatizados.",
            FunctionalRequirements =
            [
                "Permitir cadastro e gerenciamento de projetos.",
                "Permitir registrar descricao inicial, objetivo e publico-alvo do projeto.",
                "Permitir gerar perguntas de refinamento a partir do contexto informado."
            ],
            NonFunctionalRequirements =
            [
                "Garantir previsibilidade das respostas no modo Fake.",
                "Permitir execucao local sem dependencia de provedores externos."
            ],
            UseCases =
            [
                "Criar projeto",
                "Refinar ideia inicial",
                "Gerar documento tecnico inicial"
            ],
            Risks =
            [
                "Descricao inicial insuficiente para orientar o refinamento.",
                "Respostas de refinamento incompletas podem reduzir a qualidade do documento inicial."
            ],
            Metadata = new AiResponseMetadata
            {
                Provider = "Fake",
                Model = "fake-static-response"
            }
        };

        return Task.FromResult(response);
    }
}
