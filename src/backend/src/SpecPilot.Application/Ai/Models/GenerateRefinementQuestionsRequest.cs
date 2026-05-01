namespace SpecPilot.Application.Ai.Models;

public class GenerateRefinementQuestionsRequest
{
    public string ProjectName { get; init; } = string.Empty;
    public string InitialDescription { get; init; } = string.Empty;
    public string Goal { get; init; } = string.Empty;
    public string TargetAudience { get; init; } = string.Empty;
}
