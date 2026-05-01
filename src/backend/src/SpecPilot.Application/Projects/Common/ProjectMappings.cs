using SpecPilot.Domain.Entities;

namespace SpecPilot.Application.Projects.Common;

public static class ProjectMappings
{
    public static ProjectResponse ToResponse(this Project project)
    {
        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            InitialDescription = project.InitialDescription,
            Goal = project.Goal,
            TargetAudience = project.TargetAudience,
            Status = project.Status.ToString(),
            CreatedAt = project.CreatedAtUtc,
            UpdatedAt = project.UpdatedAtUtc
        };
    }
}
