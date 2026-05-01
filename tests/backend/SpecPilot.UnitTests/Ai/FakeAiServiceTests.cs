using FluentAssertions;
using SpecPilot.Application.Ai;
using SpecPilot.Application.Ai.Models;
using SpecPilot.Infrastructure.Ai;

namespace SpecPilot.UnitTests.Ai;

public class FakeAiServiceTests
{
    private readonly FakeAiService _service = new();

    [Fact]
    public async Task Should_generate_predictable_refinement_questions()
    {
        var request = new GenerateRefinementQuestionsRequest
        {
            ProjectName = "SpecPilot AI",
            InitialDescription = "Sistema para gerar especificacao inicial.",
            Goal = "Refinar requisitos",
            TargetAudience = "Analistas"
        };

        var response = await _service.GenerateRefinementQuestionsAsync(request, CancellationToken.None);

        response.Questions.Should().HaveCount(5);
        response.Questions.Should().Contain("Quem sao os principais usuarios do sistema?");
        response.Questions.Should().OnlyContain(x => !string.IsNullOrWhiteSpace(x));
    }

    [Fact]
    public async Task Should_generate_predictable_initial_project_document()
    {
        var request = new GenerateProjectDocumentRequest
        {
            ProjectName = "SpecPilot AI",
            InitialDescription = "Sistema para gerar especificacao inicial.",
            Goal = "Refinar requisitos",
            TargetAudience = "Analistas",
            Answers =
            [
                new RefinementAnswerInput
                {
                    Question = "Quem sao os usuarios?",
                    Answer = "Analistas e estudantes"
                }
            ]
        };

        var response = await _service.GenerateProjectDocumentAsync(request, CancellationToken.None);

        response.Overview.Should().NotBeNullOrWhiteSpace();
        response.FunctionalRequirements.Should().Contain("Permitir cadastro e gerenciamento de projetos.");
        response.NonFunctionalRequirements.Should().Contain("Garantir previsibilidade das respostas no modo Fake.");
        response.UseCases.Should().Contain("Criar projeto");
        response.Risks.Should().Contain("Descricao inicial insuficiente para orientar o refinamento.");
    }
}
