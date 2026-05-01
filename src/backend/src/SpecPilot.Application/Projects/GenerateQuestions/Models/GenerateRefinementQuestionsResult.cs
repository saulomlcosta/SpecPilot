namespace SpecPilot.Application.Projects.GenerateQuestions.Models;

public class GenerateRefinementQuestionsResult
{
    public Guid ProjectId { get; init; }
    public string Status { get; init; } = string.Empty;
    public IReadOnlyList<string> Questions { get; init; } = [];
}
