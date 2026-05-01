using SpecPilot.Domain.Common;

namespace SpecPilot.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
