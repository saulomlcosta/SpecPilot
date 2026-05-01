using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecPilot.Domain.Entities;

namespace SpecPilot.Infrastructure.Persistence.Configurations;

public class RefinementQuestionConfiguration : IEntityTypeConfiguration<RefinementQuestion>
{
    public void Configure(EntityTypeBuilder<RefinementQuestion> builder)
    {
        builder.ToTable("refinement_questions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.QuestionText).HasColumnType("text").IsRequired();
        builder.Property(x => x.AnswerText).HasColumnType("text");
        builder.Property(x => x.Order).IsRequired();

        builder.HasOne(x => x.Project)
            .WithMany(x => x.RefinementQuestions)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
