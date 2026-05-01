namespace SpecPilot.Application.Ai.Models;

public class GenerateProjectDocumentResponse
{
    public string Overview { get; init; } = string.Empty;
    public IReadOnlyList<string> FunctionalRequirements { get; init; } = [];
    public IReadOnlyList<string> NonFunctionalRequirements { get; init; } = [];
    public IReadOnlyList<string> UseCases { get; init; } = [];
    public IReadOnlyList<string> Risks { get; init; } = [];
}
