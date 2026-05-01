using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Projects.GetById;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;
using SpecPilot.Infrastructure.Persistence;
using SpecPilot.UnitTests.Auth;

namespace SpecPilot.UnitTests.Projects;

public class GetProjectByIdQueryHandlerTests
{
    [Fact]
    public async Task Should_return_project_when_it_belongs_to_current_user()
    {
        await using var context = CreateContext();
        var currentUserId = Guid.NewGuid();
        var project = new Project
        {
            UserId = currentUserId,
            Name = "Projeto A",
            InitialDescription = "Descricao A",
            Goal = "Objetivo A",
            TargetAudience = "Publico A",
            Status = ProjectStatus.Draft
        };

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new GetProjectByIdQueryHandler(context, new TestCurrentUserAccessor(currentUserId));
        var result = await handler.Handle(new GetProjectByIdQuery(project.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(project.Id);
    }

    [Fact]
    public async Task Should_return_not_found_when_project_belongs_to_another_user()
    {
        await using var context = CreateContext();
        var project = new Project
        {
            UserId = Guid.NewGuid(),
            Name = "Projeto A",
            InitialDescription = "Descricao A",
            Goal = "Objetivo A",
            TargetAudience = "Publico A",
            Status = ProjectStatus.Draft
        };

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new GetProjectByIdQueryHandler(context, new TestCurrentUserAccessor(Guid.NewGuid()));
        var result = await handler.Handle(new GetProjectByIdQuery(project.Id), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        result.Error.Code.Should().Be("projects.not_found");
    }

    [Fact]
    public async Task Should_return_not_found_for_unknown_project()
    {
        await using var context = CreateContext();
        var handler = new GetProjectByIdQueryHandler(context, new TestCurrentUserAccessor(Guid.NewGuid()));

        var result = await handler.Handle(new GetProjectByIdQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        result.Error.Code.Should().Be("projects.not_found");
    }

    private static SpecPilotDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SpecPilotDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SpecPilotDbContext(options);
    }
}
