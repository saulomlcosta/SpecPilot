using FluentAssertions;
using SpecPilot.Application.Projects.AnswerQuestions;

namespace SpecPilot.UnitTests.Projects;

public class AnswerRefinementQuestionsCommandValidatorTests
{
    private readonly AnswerRefinementQuestionsCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_command_is_invalid()
    {
        var command = new AnswerRefinementQuestionsCommand
        {
            ProjectId = Guid.Empty,
            Answers = []
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(AnswerRefinementQuestionsCommand.ProjectId));
        result.Errors.Should().Contain(x => x.PropertyName == nameof(AnswerRefinementQuestionsCommand.Answers));
    }

    [Fact]
    public void Should_fail_when_answer_item_is_invalid()
    {
        var command = new AnswerRefinementQuestionsCommand
        {
            ProjectId = Guid.NewGuid(),
            Answers =
            [
                new AnswerRefinementQuestionItem
                {
                    QuestionId = Guid.Empty,
                    Answer = string.Empty
                }
            ]
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Answers[0].QuestionId");
        result.Errors.Should().Contain(x => x.PropertyName == "Answers[0].Answer");
    }
}
