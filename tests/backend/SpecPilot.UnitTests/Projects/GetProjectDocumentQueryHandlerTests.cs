using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Projects.GetDocument;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;
using SpecPilot.Infrastructure.Persistence;
using SpecPilot.UnitTests.Auth;

namespace SpecPilot.UnitTests.Projects;

public class GetProjectDocumentQueryHandlerTests
{
    [Fact]
    public async Task Should_return_document_for_owned_project()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var project = CreateProject(userId);

        context.Projects.Add(project);
        context.ProjectDocuments.Add(new ProjectDocument
        {
            ProjectId = project.Id,
            Overview = "Overview",
            FunctionalRequirements = "[\"RF1\",\"RF2\"]",
            NonFunctionalRequirements = "[\"RNF1\"]",
            UseCases = "[\"UC1\"]",
            Risks = "[\"R1\"]"
        });
        await context.SaveChangesAsync();

        var handler = new GetProjectDocumentQueryHandler(context, new TestCurrentUserAccessor(userId));

        var result = await handler.Handle(new GetProjectDocumentQuery(project.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.ProjectId.Should().Be(project.Id);
        result.Value.FunctionalRequirements.Should().Contain("RF1");
        result.Value.NonFunctionalRequirements.Should().Contain("RNF1");
    }

    [Fact]
    public async Task Should_return_not_found_for_project_from_another_user()
    {
        await using var context = CreateContext();
        var project = CreateProject(Guid.NewGuid());

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new GetProjectDocumentQueryHandler(context, new TestCurrentUserAccessor(Guid.NewGuid()));

        var result = await handler.Handle(new GetProjectDocumentQuery(project.Id), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        result.Error.Code.Should().Be("projects.not_found");
    }

    [Fact]
    public async Task Should_return_not_found_when_document_does_not_exist()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var project = CreateProject(userId);

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new GetProjectDocumentQueryHandler(context, new TestCurrentUserAccessor(userId));

        var result = await handler.Handle(new GetProjectDocumentQuery(project.Id), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        result.Error.Code.Should().Be("projects.document_not_found");
    }

    private static Project CreateProject(Guid userId)
    {
        return new Project
        {
            UserId = userId,
            Name = "SpecPilot AI",
            InitialDescription = "Sistema para refinar requisitos.",
            Goal = "Gerar documento inicial.",
            TargetAudience = "Analistas",
            Status = ProjectStatus.DocumentGenerated
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
