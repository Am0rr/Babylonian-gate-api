using BG.Domain.Entities.Identity;
using BG.Domain.Entities.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BG.Infra.Persistence.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Sessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.ClientId).IsRequired();
        builder.Property(s => s.InstructorId).IsRequired();
        builder.Property(s => s.StartedAt).IsRequired();
        builder.Property(s => s.EndedAt).IsRequired(false);
        builder.Property(s => s.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(s => s.ClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(s => s.InstructorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Items)
            .WithOne(si => si.Session)
            .HasForeignKey(si => si.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}