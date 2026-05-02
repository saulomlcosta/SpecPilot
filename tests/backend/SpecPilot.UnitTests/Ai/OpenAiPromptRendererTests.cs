using FluentAssertions;
using Microsoft.Extensions.Options;
using SpecPilot.Infrastructure.Ai;

namespace SpecPilot.UnitTests.Ai;

public class OpenAiPromptRendererTests
{
    [Fact]
    public async Task Should_render_refinement_questions_prompt_with_placeholders()
    {
        var promptsRoot = CreatePromptsRoot(
            "generate-refinement-questions.costar.md",
            """
            Projeto: {{ProjectName}}
            Descricao: {{InitialDescription}}
            Objetivo: {{Goal}}
            Publico: {{TargetAudience}}
            """);

        var renderer = CreateRenderer(promptsRoot);

        var prompt = await renderer.RenderRefinementQuestionsPromptAsync(
            "SpecPilot AI",
            "Sistema para refinar requisitos.",
            "Esclarecer escopo",
            "Analistas",
            CancellationToken.None);

        prompt.Should().Contain("Projeto: SpecPilot AI");
        prompt.Should().Contain("Descricao: Sistema para refinar requisitos.");
        prompt.Should().Contain("Objetivo: Esclarecer escopo");
        prompt.Should().Contain("Publico: Analistas");
    }

    [Fact]
    public async Task Should_render_project_document_prompt_with_answers()
    {
        var promptsRoot = CreatePromptsRoot(
            "generate-project-document.costar.md",
            """
            Projeto: {{ProjectName}}
            Respostas:
            {{RefinementAnswers}}
            """);

        var renderer = CreateRenderer(promptsRoot);

        var prompt = await renderer.RenderProjectDocumentPromptAsync(
            "SpecPilot AI",
            "Sistema para refinar requisitos.",
            "Esclarecer escopo",
            "Analistas",
            [
                ("Quem sao os usuarios?", "Analistas"),
                ("Qual o objetivo?", "Gerar documento")
            ],
            CancellationToken.None);

        prompt.Should().Contain("Projeto: SpecPilot AI");
        prompt.Should().Contain("- Pergunta: Quem sao os usuarios?");
        prompt.Should().Contain("- Resposta: Analistas");
        prompt.Should().Contain("- Pergunta: Qual o objetivo?");
        prompt.Should().Contain("- Resposta: Gerar documento");
    }

    private static OpenAiPromptRenderer CreateRenderer(string promptsRoot)
    {
        return new OpenAiPromptRenderer(Options.Create(new AiOptions
        {
            PromptsRoot = promptsRoot
        }));
    }

    private static string CreatePromptsRoot(string fileName, string content)
    {
        var root = Path.Combine(Path.GetTempPath(), "specpilot-prompts", Guid.NewGuid().ToString("N"), "runtime");
        Directory.CreateDirectory(root);
        File.WriteAllText(Path.Combine(root, fileName), content);
        return Directory.GetParent(root)!.FullName;
    }
}
