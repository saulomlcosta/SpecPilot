using MediatR;
using SpecPilot.Application.Projects.AnswerQuestions.Models;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.AnswerQuestions;

public class AnswerRefinementQuestionsCommand : IRequest<Result<AnswerRefinementQuestionsResult>>
{
    public Guid ProjectId { get; init; }
    public IReadOnlyList<AnswerRefinementQuestionItem> Answers { get; init; } = [];
}
