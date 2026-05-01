using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecPilot.Domain.Entities;

namespace SpecPilot.Infrastructure.Persistence.Configurations;

public class ProjectDocumentConfiguration : IEntityTypeConfiguration<ProjectDocument>
{
    public void Configure(EntityTypeBuilder<ProjectDocument> builder)
    {
        builder.ToTable("project_documents");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Overview).HasColumnType("text").IsRequired();
        builder.Property(x => x.FunctionalRequirements).HasColumnType("text").IsRequired();
        builder.Property(x => x.NonFunctionalRequirements).HasColumnType("text").IsRequired();
        builder.Property(x => x.UseCases).HasColumnType("text").IsRequired();
        builder.Property(x => x.Risks).HasColumnType("text").IsRequired();

        builder.HasOne(x => x.Project)
            .WithOne(x => x.ProjectDocument)
            .HasForeignKey<ProjectDocument>(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
