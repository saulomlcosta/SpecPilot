using Microsoft.EntityFrameworkCore;
using SpecPilot.Domain.Entities;

namespace SpecPilot.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Project> Projects { get; }
    DbSet<RefinementQuestion> RefinementQuestions { get; }
    DbSet<ProjectDocument> ProjectDocuments { get; }
    DbSet<AiInteractionLog> AiInteractionLogs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
