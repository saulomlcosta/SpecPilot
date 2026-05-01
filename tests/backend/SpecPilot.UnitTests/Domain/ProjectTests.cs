using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;

namespace SpecPilot.UnitTests.Domain;

public class ProjectTests
{
    [Fact]
    public void New_project_should_start_in_draft_status()
    {
        var project = new Project();

        Assert.Equal(ProjectStatus.Draft, project.Status);
    }
}
