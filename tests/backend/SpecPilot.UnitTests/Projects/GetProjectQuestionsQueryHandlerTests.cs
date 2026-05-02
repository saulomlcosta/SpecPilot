using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Projects.GetQuestions;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;
using SpecPilot.Infrastructure.Persistence;
using SpecPilot.UnitTests.Auth;

namespace SpecPilot.UnitTests.Projects;

public class GetProjectQuestionsQueryHandlerTests
{
    [Fact]
    public async Task Should_return_generated_questions_for_owned_project()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var project = CreateProject(userId);
        var firstQuestion = CreateQuestion(project, 1, "Pergunta 1");
        var secondQuestion = CreateQuestion(project, 2, "Pergunta 2");

        context.Projects.Add(project);
        context.RefinementQuestions.AddRange(firstQuestion, secondQuestion);
        await context.SaveChangesAsync();

        var handler = new GetProjectQuestionsQueryHandler(context, new TestCurrentUserAccessor(userId));

        var result = await handler.Handle(new GetProjectQuestionsQuery(project.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.ProjectId.Should().Be(project.Id);
        result.Value.Status.Should().Be(ProjectStatus.QuestionsGenerated.ToString());
        result.Value.Questions.Should().HaveCount(2);
        result.Value.Questions[0].Id.Should().Be(firstQuestion.Id);
        result.Value.Questions[0].Order.Should().Be(1);
        result.Value.Questions[0].QuestionText.Should().Be("Pergunta 1");
    }

    [Fact]
    public async Task Should_return_not_found_for_project_from_another_user()
    {
        await using var context = CreateContext();
        var project = CreateProject(Guid.NewGuid());

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new GetProjectQuestionsQueryHandler(context, new TestCurrentUserAccessor(Guid.NewGuid()));

        var result = await handler.Handle(new GetProjectQuestionsQuery(project.Id), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        result.Error.Code.Should().Be("projects.not_found");
    }

    [Fact]
    public async Task Should_return_not_found_when_questions_were_not_generated()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var project = CreateProject(userId);

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new GetProjectQuestionsQueryHandler(context, new TestCurrentUserAccessor(userId));

        var result = await handler.Handle(new GetProjectQuestionsQuery(project.Id), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        result.Error.Code.Should().Be("projects.questions_not_found");
    }

    private static Project CreateProject(Guid userId)
    {
        return new Project
        {
            UserId = userId,
            Name = "SpecPilot AI",
            InitialDescription = "Sistema para refinar requisitos.",
            Goal = "Ler perguntas geradas.",
            TargetAudience = "Analistas",
            Status = ProjectStatus.QuestionsGenerated
        };
    }

    private static RefinementQuestion CreateQuestion(Project project, int order, string questionText)
    {
        return new RefinementQuestion
        {
            ProjectId = project.Id,
            Order = order,
            QuestionText = questionText
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
