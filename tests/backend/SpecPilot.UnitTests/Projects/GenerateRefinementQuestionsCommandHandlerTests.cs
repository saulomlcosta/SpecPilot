using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Projects.GenerateQuestions;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;
using SpecPilot.Infrastructure.Ai;
using SpecPilot.Infrastructure.Persistence;
using SpecPilot.UnitTests.Auth;

namespace SpecPilot.UnitTests.Projects;

public class GenerateRefinementQuestionsCommandHandlerTests
{
    [Fact]
    public async Task Should_generate_questions_for_owned_draft_project()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var project = new Project
        {
            UserId = userId,
            Name = "SpecPilot AI",
            InitialDescription = "Sistema para refinar requisitos.",
            Goal = "Gerar perguntas iniciais.",
            TargetAudience = "Analistas",
            Status = ProjectStatus.Draft
        };

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new GenerateRefinementQuestionsCommandHandler(
            context,
            new TestCurrentUserAccessor(userId),
            new FakeAiService());

        var result = await handler.Handle(new GenerateRefinementQuestionsCommand(project.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Status.Should().Be(ProjectStatus.QuestionsGenerated.ToString());
        result.Value.Questions.Should().HaveCount(5);

        context.RefinementQuestions.Should().HaveCount(5);
        context.RefinementQuestions.Should().BeInAscendingOrder(x => x.Order);
        context.AiInteractionLogs.Should().ContainSingle();
        context.Projects.Single().Status.Should().Be(ProjectStatus.QuestionsGenerated);
        context.Projects.Single().UpdatedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_return_not_found_for_project_from_another_user()
    {
        await using var context = CreateContext();
        var project = new Project
        {
            UserId = Guid.NewGuid(),
            Name = "SpecPilot AI",
            InitialDescription = "Sistema para refinar requisitos.",
            Goal = "Gerar perguntas iniciais.",
            TargetAudience = "Analistas",
            Status = ProjectStatus.Draft
        };

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new GenerateRefinementQuestionsCommandHandler(
            context,
            new TestCurrentUserAccessor(Guid.NewGuid()),
            new FakeAiService());

        var result = await handler.Handle(new GenerateRefinementQuestionsCommand(project.Id), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        result.Error.Code.Should().Be("projects.not_found");
    }

    [Fact]
    public async Task Should_return_conflict_when_project_status_is_not_draft()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var project = new Project
        {
            UserId = userId,
            Name = "SpecPilot AI",
            InitialDescription = "Sistema para refinar requisitos.",
            Goal = "Gerar perguntas iniciais.",
            TargetAudience = "Analistas",
            Status = ProjectStatus.QuestionsGenerated
        };

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new GenerateRefinementQuestionsCommandHandler(
            context,
            new TestCurrentUserAccessor(userId),
            new FakeAiService());

        var result = await handler.Handle(new GenerateRefinementQuestionsCommand(project.Id), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.Conflict);
        result.Error.Code.Should().Be("projects.invalid_status_for_question_generation");
        context.RefinementQuestions.Should().BeEmpty();
        context.AiInteractionLogs.Should().BeEmpty();
    }

    private static SpecPilotDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SpecPilotDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SpecPilotDbContext(options);
    }
}
