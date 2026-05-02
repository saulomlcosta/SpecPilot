using FluentValidation;

namespace SpecPilot.Application.Projects.AnswerQuestions;

public class AnswerRefinementQuestionsCommandValidator : AbstractValidator<AnswerRefinementQuestionsCommand>
{
    public AnswerRefinementQuestionsCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty();

        RuleFor(x => x.Answers)
            .NotEmpty();

        RuleForEach(x => x.Answers)
            .ChildRules(answer =>
            {
                answer.RuleFor(x => x.QuestionId)
                    .NotEmpty();

                answer.RuleFor(x => x.Answer)
                    .NotEmpty();
            });
    }
}
