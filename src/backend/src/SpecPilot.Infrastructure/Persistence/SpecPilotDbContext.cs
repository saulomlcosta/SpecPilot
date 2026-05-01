using Microsoft.EntityFrameworkCore;
using SpecPilot.Domain.Entities;

namespace SpecPilot.Infrastructure.Persistence;

public class SpecPilotDbContext(DbContextOptions<SpecPilotDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<RefinementQuestion> RefinementQuestions => Set<RefinementQuestion>();
    public DbSet<ProjectDocument> ProjectDocuments => Set<ProjectDocument>();
    public DbSet<AiInteractionLog> AiInteractionLogs => Set<AiInteractionLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SpecPilotDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
