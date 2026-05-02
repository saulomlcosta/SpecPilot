using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Projects.GenerateDocument;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;
using SpecPilot.Infrastructure.Ai;
using SpecPilot.Infrastructure.Persistence;
using SpecPilot.UnitTests.Auth;

namespace SpecPilot.UnitTests.Projects;

public class GenerateProjectDocumentCommandHandlerTests
{
    [Fact]
    public async Task Should_generate_document_for_owned_project_with_answered_questions()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var project = CreateProject(userId, ProjectStatus.QuestionsAnswered);
        var firstQuestion = CreateAnsweredQuestion(project, 1, "Quem sao os usuarios?", "Analistas.");
        var secondQuestion = CreateAnsweredQuestion(project, 2, "Qual o objetivo?", "Documentar requisitos.");

        context.Projects.Add(project);
        context.RefinementQuestions.AddRange(firstQuestion, secondQuestion);
        await context.SaveChangesAsync();

        var handler = new GenerateProjectDocumentCommandHandler(
            context,
            new TestCurrentUserAccessor(userId),
            new FakeAiService());

        var result = await handler.Handle(new GenerateProjectDocumentCommand(project.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.ProjectId.Should().Be(project.Id);
        result.Value.Status.Should().Be(ProjectStatus.DocumentGenerated.ToString());
        result.Value.Overview.Should().NotBeNullOrWhiteSpace();
        result.Value.FunctionalRequirements.Should().NotBeEmpty();

        context.ProjectDocuments.Should().ContainSingle();
        context.ProjectDocuments.Single().ProjectId.Should().Be(project.Id);
        context.ProjectDocuments.Single().Overview.Should().NotBeNullOrWhiteSpace();
        context.AiInteractionLogs.Should().ContainSingle(x => x.ProjectId == project.Id && x.InteractionType == "GenerateProjectDocument");
        context.Projects.Single().Status.Should().Be(ProjectStatus.DocumentGenerated);
    }

    [Fact]
    public async Task Should_return_not_found_for_project_from_another_user()
    {
        await using var context = CreateContext();
        var ownerId = Guid.NewGuid();
        var project = CreateProject(ownerId, ProjectStatus.QuestionsAnswered);

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new GenerateProjectDocumentCommandHandler(
            context,
            new TestCurrentUserAccessor(Guid.NewGuid()),
            new FakeAiService());

        var result = await handler.Handle(new GenerateProjectDocumentCommand(project.Id), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        result.Error.Code.Should().Be("projects.not_found");
    }

    [Fact]
    public async Task Should_return_conflict_when_project_status_is_not_questions_answered()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var project = CreateProject(userId, ProjectStatus.QuestionsGenerated);

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new GenerateProjectDocumentCommandHandler(
            context,
            new TestCurrentUserAccessor(userId),
            new FakeAiService());

        var result = await handler.Handle(new GenerateProjectDocumentCommand(project.Id), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.Conflict);
        result.Error.Code.Should().Be("projects.invalid_status_for_document_generation");
    }

    [Fact]
    public async Task Should_return_conflict_when_document_already_exists()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var project = CreateProject(userId, ProjectStatus.QuestionsAnswered);

        context.Projects.Add(project);
        context.ProjectDocuments.Add(new ProjectDocument
        {
            ProjectId = project.Id,
            Overview = "Overview",
            FunctionalRequirements = "[\"RF1\"]",
            NonFunctionalRequirements = "[\"RNF1\"]",
            UseCases = "[\"UC1\"]",
            Risks = "[\"R1\"]"
        });
        await context.SaveChangesAsync();

        var handler = new GenerateProjectDocumentCommandHandler(
            context,
            new TestCurrentUserAccessor(userId),
            new FakeAiService());

        var result = await handler.Handle(new GenerateProjectDocumentCommand(project.Id), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.Conflict);
        result.Error.Code.Should().Be("projects.document_already_generated");
    }

    private static Project CreateProject(Guid userId, ProjectStatus status)
    {
        return new Project
        {
            UserId = userId,
            Name = "SpecPilot AI",
            InitialDescription = "Sistema para refinar requisitos.",
            Goal = "Gerar documento inicial.",
            TargetAudience = "Analistas",
            Status = status
        };
    }

    private static RefinementQuestion CreateAnsweredQuestion(Project project, int order, string text, string answer)
    {
        return new RefinementQuestion
        {
            ProjectId = project.Id,
            Order = order,
            QuestionText = text,
            AnswerText = answer,
            AnsweredAtUtc = DateTime.UtcNow
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
