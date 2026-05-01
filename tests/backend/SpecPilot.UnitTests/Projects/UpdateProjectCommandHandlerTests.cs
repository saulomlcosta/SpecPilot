using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Projects.Update;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;
using SpecPilot.Infrastructure.Persistence;
using SpecPilot.UnitTests.Auth;

namespace SpecPilot.UnitTests.Projects;

public class UpdateProjectCommandHandlerTests
{
    [Fact]
    public async Task Should_update_owned_project()
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

        var handler = new UpdateProjectCommandHandler(context, new TestCurrentUserAccessor(currentUserId));
        var result = await handler.Handle(new UpdateProjectCommand
        {
            Id = project.Id,
            Name = "Projeto Atualizado",
            InitialDescription = "Descricao Atualizada",
            Goal = "Objetivo Atualizado",
            TargetAudience = "Publico Atualizado"
        }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be("Projeto Atualizado");
        context.Projects.Single().InitialDescription.Should().Be("Descricao Atualizada");
        context.Projects.Single().Goal.Should().Be("Objetivo Atualizado");
        context.Projects.Single().TargetAudience.Should().Be("Publico Atualizado");
        context.Projects.Single().Status.Should().Be(ProjectStatus.Draft);
        context.Projects.Single().UpdatedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_return_not_found_when_updating_project_from_another_user()
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

        var handler = new UpdateProjectCommandHandler(context, new TestCurrentUserAccessor(Guid.NewGuid()));
        var result = await handler.Handle(new UpdateProjectCommand
        {
            Id = project.Id,
            Name = "Projeto Atualizado",
            InitialDescription = "Descricao Atualizada",
            Goal = "Objetivo Atualizado",
            TargetAudience = "Publico Atualizado"
        }, CancellationToken.None);

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
