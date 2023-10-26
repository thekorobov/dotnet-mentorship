using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Persistence.Configurations;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.Property(v => v.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.Address)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.SeatingCapacity)
            .IsRequired();

        builder.HasMany(v => v.Halls)
            .WithOne(h => h.Venue)
            .HasForeignKey(h => h.VenueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(v => v.Events)
            .WithOne(e => e.Venue)
            .HasForeignKey(e => e.VenueId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.User)
            .WithMany(u => u.Venues)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}