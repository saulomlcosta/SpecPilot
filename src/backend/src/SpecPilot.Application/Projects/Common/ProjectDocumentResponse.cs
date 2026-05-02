namespace SpecPilot.Application.Projects.Common;

public class ProjectDocumentResponse
{
    public Guid ProjectId { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Overview { get; init; } = string.Empty;
    public IReadOnlyList<string> FunctionalRequirements { get; init; } = [];
    public IReadOnlyList<string> NonFunctionalRequirements { get; init; } = [];
    public IReadOnlyList<string> UseCases { get; init; } = [];
    public IReadOnlyList<string> Risks { get; init; } = [];
}
