using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Projects.List;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;
using SpecPilot.Infrastructure.Persistence;
using SpecPilot.UnitTests.Auth;

namespace SpecPilot.UnitTests.Projects;

public class GetProjectsQueryHandlerTests
{
    [Fact]
    public async Task Should_list_only_projects_from_current_user()
    {
        await using var context = CreateContext();
        var currentUserId = Guid.NewGuid();

        context.Projects.AddRange(
            new Project
            {
                UserId = currentUserId,
                Name = "Projeto A",
                InitialDescription = "Descricao A",
                Goal = "Objetivo A",
                TargetAudience = "Publico A",
                Status = ProjectStatus.Draft
            },
            new Project
            {
                UserId = Guid.NewGuid(),
                Name = "Projeto B",
                InitialDescription = "Descricao B",
                Goal = "Objetivo B",
                TargetAudience = "Publico B",
                Status = ProjectStatus.Draft
            });

        await context.SaveChangesAsync();

        var handler = new GetProjectsQueryHandler(context, new TestCurrentUserAccessor(currentUserId));

        var result = await handler.Handle(new GetProjectsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value![0].Name.Should().Be("Projeto A");
    }

    private static SpecPilotDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SpecPilotDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SpecPilotDbContext(options);
    }
}
