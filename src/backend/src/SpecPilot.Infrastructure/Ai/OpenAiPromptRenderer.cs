using System.Text;
using Microsoft.Extensions.Options;

namespace SpecPilot.Infrastructure.Ai;

public class OpenAiPromptRenderer
{
    private readonly AiOptions _options;

    public OpenAiPromptRenderer(IOptions<AiOptions> options)
    {
        _options = options.Value;
    }

    public async Task<string> RenderRefinementQuestionsPromptAsync(
        string projectName,
        string initialDescription,
        string goal,
        string targetAudience,
        CancellationToken cancellationToken)
    {
        var template = await LoadPromptAsync("generate-refinement-questions.costar.md", cancellationToken);

        return template
            .Replace("{{ProjectName}}", projectName)
            .Replace("{{InitialDescription}}", initialDescription)
            .Replace("{{Goal}}", goal)
            .Replace("{{TargetAudience}}", targetAudience);
    }

    public async Task<string> RenderProjectDocumentPromptAsync(
        string projectName,
        string initialDescription,
        string goal,
        string targetAudience,
        IReadOnlyList<(string Question, string Answer)> answers,
        CancellationToken cancellationToken)
    {
        var template = await LoadPromptAsync("generate-project-document.costar.md", cancellationToken);
        var renderedAnswers = RenderAnswers(answers);

        return template
            .Replace("{{ProjectName}}", projectName)
            .Replace("{{InitialDescription}}", initialDescription)
            .Replace("{{Goal}}", goal)
            .Replace("{{TargetAudience}}", targetAudience)
            .Replace("{{RefinementAnswers}}", renderedAnswers);
    }

    private async Task<string> LoadPromptAsync(string fileName, CancellationToken cancellationToken)
    {
        var promptFile = ResolvePromptFile(fileName);
        return await File.ReadAllTextAsync(promptFile, cancellationToken);
    }

    private string ResolvePromptFile(string fileName)
    {
        if (!string.IsNullOrWhiteSpace(_options.PromptsRoot))
        {
            return Path.Combine(_options.PromptsRoot, "runtime", fileName);
        }

        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            var candidate = Path.Combine(current.FullName, "prompts", "runtime", fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }

            current = current.Parent;
        }

        throw new AiProviderException($"Prompt runtime '{fileName}' nao encontrado.");
    }

    private static string RenderAnswers(IReadOnlyList<(string Question, string Answer)> answers)
    {
        var builder = new StringBuilder();

        foreach (var (question, answer) in answers)
        {
            builder.AppendLine($"- Pergunta: {question}");
            builder.AppendLine($"- Resposta: {answer}");
        }

        return builder.ToString().TrimEnd();
    }
}
