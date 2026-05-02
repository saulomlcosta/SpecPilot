namespace SpecPilot.Application.Projects.GetQuestions.Models;

public class GetProjectQuestionsResult
{
    public Guid ProjectId { get; init; }
    public string Status { get; init; } = string.Empty;
    public IReadOnlyList<ProjectQuestionResponse> Questions { get; init; } = [];
}
