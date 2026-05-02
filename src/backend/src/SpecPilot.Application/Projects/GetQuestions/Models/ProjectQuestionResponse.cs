namespace SpecPilot.Application.Projects.GetQuestions.Models;

public class ProjectQuestionResponse
{
    public Guid Id { get; init; }
    public int Order { get; init; }
    public string QuestionText { get; init; } = string.Empty;
    public string? Answer { get; init; }
}
