namespace SpecPilot.Application.Ai.Models;

public class GenerateRefinementQuestionsResponse
{
    public IReadOnlyList<string> Questions { get; init; } = [];
    public AiResponseMetadata? Metadata { get; init; }
}
