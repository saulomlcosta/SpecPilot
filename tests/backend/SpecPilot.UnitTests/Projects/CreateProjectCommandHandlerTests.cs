using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Projects.Create;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;
using SpecPilot.Infrastructure.Persistence;
using SpecPilot.UnitTests.Auth;

namespace SpecPilot.UnitTests.Projects;

public class CreateProjectCommandHandlerTests
{
    [Fact]
    public async Task Should_create_project_for_authenticated_user()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var handler = new CreateProjectCommandHandler(context, new TestCurrentUserAccessor(userId));

        var command = new CreateProjectCommand
        {
            Name = "SpecPilot",
            InitialDescription = "Sistema para gerar especificacao inicial.",
            Goal = "Ajudar a descobrir requisitos.",
            TargetAudience = "Analistas de sistemas"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be("SpecPilot");
        result.Value.Status.Should().Be(ProjectStatus.Draft.ToString());
        context.Projects.Should().ContainSingle();
        context.Projects.Single().UserId.Should().Be(userId);
        context.Projects.Single().Status.Should().Be(ProjectStatus.Draft);
    }

    private static SpecPilotDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SpecPilotDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SpecPilotDbContext(options);
    }
}
