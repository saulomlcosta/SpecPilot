using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecPilot.Domain.Entities;

namespace SpecPilot.Infrastructure.Persistence.Configurations;

public class AiInteractionLogConfiguration : IEntityTypeConfiguration<AiInteractionLog>
{
    public void Configure(EntityTypeBuilder<AiInteractionLog> builder)
    {
        builder.ToTable("ai_interaction_logs");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.InteractionType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Provider).HasMaxLength(100).IsRequired();
        builder.Property(x => x.PromptName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.InputPayload).HasColumnType("text").IsRequired();
        builder.Property(x => x.OutputPayload).HasColumnType("text").IsRequired();
        builder.Property(x => x.IsSuccessful).IsRequired();

        builder.HasOne(x => x.Project)
            .WithMany(x => x.AiInteractionLogs)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
