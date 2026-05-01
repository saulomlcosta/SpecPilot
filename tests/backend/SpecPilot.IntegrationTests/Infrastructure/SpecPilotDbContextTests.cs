using Microsoft.EntityFrameworkCore;
using SpecPilot.Infrastructure.Persistence;

namespace SpecPilot.IntegrationTests.Infrastructure;

public class SpecPilotDbContextTests
{
    [Fact]
    public void Db_context_should_expose_required_sets()
    {
        var options = new DbContextOptionsBuilder<SpecPilotDbContext>()
            .UseInMemoryDatabase(databaseName: "specpilot-dbcontext-test")
            .Options;

        using var context = new SpecPilotDbContext(options);

        Assert.NotNull(context.Users);
        Assert.NotNull(context.Projects);
        Assert.NotNull(context.RefinementQuestions);
        Assert.NotNull(context.ProjectDocuments);
        Assert.NotNull(context.AiInteractionLogs);
    }
}
