using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Projects.AnswerQuestions;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;
using SpecPilot.Infrastructure.Persistence;
using SpecPilot.UnitTests.Auth;

namespace SpecPilot.UnitTests.Projects;

public class AnswerRefinementQuestionsCommandHandlerTests
{
    [Fact]
    public async Task Should_answer_all_questions_for_owned_project()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var project = CreateProject(userId, ProjectStatus.QuestionsGenerated);
        var firstQuestion = CreateQuestion(project, 1, "Quem sao os usuarios?");
        var secondQuestion = CreateQuestion(project, 2, "Qual o objetivo principal?");

        context.Projects.Add(project);
        context.RefinementQuestions.AddRange(firstQuestion, secondQuestion);
        await context.SaveChangesAsync();

        var handler = new AnswerRefinementQuestionsCommandHandler(context, new TestCurrentUserAccessor(userId));
        var command = new AnswerRefinementQuestionsCommand
        {
            ProjectId = project.Id,
            Answers =
            [
                new AnswerRefinementQuestionItem
                {
                    QuestionId = firstQuestion.Id,
                    Answer = "Analistas e gestores."
                },
                new AnswerRefinementQuestionItem
                {
                    QuestionId = secondQuestion.Id,
                    Answer = "Refinar requisitos do produto."
                }
            ]
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.ProjectId.Should().Be(project.Id);
        result.Value.Status.Should().Be(ProjectStatus.QuestionsAnswered.ToString());

        var persistedProject = context.Projects.Single();
        persistedProject.Status.Should().Be(ProjectStatus.QuestionsAnswered);
        persistedProject.UpdatedAtUtc.Should().NotBeNull();

        context.RefinementQuestions.Should().OnlyContain(x => !string.IsNullOrWhiteSpace(x.AnswerText));
        context.RefinementQuestions.Should().OnlyContain(x => x.AnsweredAtUtc.HasValue);
    }

    [Fact]
    public async Task Should_return_unauthorized_when_user_is_not_authenticated()
    {
        await using var context = CreateContext();
        var project = CreateProject(Guid.NewGuid(), ProjectStatus.QuestionsGenerated);

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new AnswerRefinementQuestionsCommandHandler(context, new TestCurrentUserAccessor(null));

        var result = await handler.Handle(
            new AnswerRefinementQuestionsCommand
            {
                ProjectId = project.Id,
                Answers =
                [
                    new AnswerRefinementQuestionItem
                    {
                        QuestionId = Guid.NewGuid(),
                        Answer = "Resposta"
                    }
                ]
            },
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.Unauthorized);
        result.Error.Code.Should().Be("projects.unauthenticated");
    }

    [Fact]
    public async Task Should_return_not_found_for_project_from_another_user()
    {
        await using var context = CreateContext();
        var ownerId = Guid.NewGuid();
        var project = CreateProject(ownerId, ProjectStatus.QuestionsGenerated);
        var question = CreateQuestion(project, 1, "Quem sao os usuarios?");

        context.Projects.Add(project);
        context.RefinementQuestions.Add(question);
        await context.SaveChangesAsync();

        var handler = new AnswerRefinementQuestionsCommandHandler(context, new TestCurrentUserAccessor(Guid.NewGuid()));

        var result = await handler.Handle(
            new AnswerRefinementQuestionsCommand
            {
                ProjectId = project.Id,
                Answers =
                [
                    new AnswerRefinementQuestionItem
                    {
                        QuestionId = question.Id,
                        Answer = "Resposta"
                    }
                ]
            },
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        result.Error.Code.Should().Be("projects.not_found");
    }

    [Fact]
    public async Task Should_return_conflict_when_project_status_is_invalid()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var project = CreateProject(userId, ProjectStatus.Draft);

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new AnswerRefinementQuestionsCommandHandler(context, new TestCurrentUserAccessor(userId));

        var result = await handler.Handle(
            new AnswerRefinementQuestionsCommand
            {
                ProjectId = project.Id,
                Answers =
                [
                    new AnswerRefinementQuestionItem
                    {
                        QuestionId = Guid.NewGuid(),
                        Answer = "Resposta"
                    }
                ]
            },
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.Conflict);
        result.Error.Code.Should().Be("projects.invalid_status_for_question_answering");
    }

    [Fact]
    public async Task Should_return_conflict_when_questions_were_not_generated()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var project = CreateProject(userId, ProjectStatus.QuestionsGenerated);

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new AnswerRefinementQuestionsCommandHandler(context, new TestCurrentUserAccessor(userId));

        var result = await handler.Handle(
            new AnswerRefinementQuestionsCommand
            {
                ProjectId = project.Id,
                Answers =
                [
                    new AnswerRefinementQuestionItem
                    {
                        QuestionId = Guid.NewGuid(),
                        Answer = "Resposta"
                    }
                ]
            },
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.Conflict);
        result.Error.Code.Should().Be("projects.questions_not_generated");
    }

    [Fact]
    public async Task Should_return_validation_error_when_answers_are_incomplete()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var project = CreateProject(userId, ProjectStatus.QuestionsGenerated);
        var firstQuestion = CreateQuestion(project, 1, "Pergunta 1");
        var secondQuestion = CreateQuestion(project, 2, "Pergunta 2");

        context.Projects.Add(project);
        context.RefinementQuestions.AddRange(firstQuestion, secondQuestion);
        await context.SaveChangesAsync();

        var handler = new AnswerRefinementQuestionsCommandHandler(context, new TestCurrentUserAccessor(userId));

        var result = await handler.Handle(
            new AnswerRefinementQuestionsCommand
            {
                ProjectId = project.Id,
                Answers =
                [
                    new AnswerRefinementQuestionItem
                    {
                        QuestionId = firstQuestion.Id,
                        Answer = "Resposta parcial"
                    }
                ]
            },
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.Validation);
        result.Error.Code.Should().Be("projects.incomplete_answers");
    }

    private static Project CreateProject(Guid userId, ProjectStatus status)
    {
        return new Project
        {
            UserId = userId,
            Name = "SpecPilot AI",
            InitialDescription = "Sistema para refinar requisitos.",
            Goal = "Refinar requisitos.",
            TargetAudience = "Analistas",
            Status = status
        };
    }

    private static RefinementQuestion CreateQuestion(Project project, int order, string text)
    {
        return new RefinementQuestion
        {
            ProjectId = project.Id,
            Order = order,
            QuestionText = text
        };
    }

    private static SpecPilotDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SpecPilotDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SpecPilotDbContext(options);
    }
}
