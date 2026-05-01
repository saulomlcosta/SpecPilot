using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecPilot.Domain.Entities;

namespace SpecPilot.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
        builder.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();
    }
}
