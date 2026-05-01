using Microsoft.EntityFrameworkCore;
using SpecPilot.Domain.Entities;

namespace SpecPilot.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
