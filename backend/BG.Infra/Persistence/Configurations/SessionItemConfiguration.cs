using BG.Domain.Entities.Inventory;
using BG.Domain.Entities.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BG.Infra.Persistence.Configurations;

public class SessionItemConfiguration : IEntityTypeConfiguration<SessionItem>
{
    public void Configure(EntityTypeBuilder<SessionItem> builder)
    {
        builder.ToTable("SessionItems");

        builder.HasKey(si => si.Id);

        builder.Property(si => si.CreatedAt).IsRequired();
        builder.Property(si => si.RoundsFired).IsRequired();

        builder.HasOne<Weapon>()
            .WithMany()
            .HasForeignKey(si => si.WeaponId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<AmmoCrate>()
            .WithMany()
            .HasForeignKey(si => si.AmmoCrateId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}