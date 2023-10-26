using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Persistence.Configurations;

public class HallConfiguration : IEntityTypeConfiguration<Hall>
{
    public void Configure(EntityTypeBuilder<Hall> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.Property(h => h.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(h => h.SeatingCapacity)
            .IsRequired();

        builder.HasOne(h => h.Venue)
            .WithMany(v => v.Halls)
            .HasForeignKey(h => h.VenueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.Event)
            .WithOne(e => e.Hall)
            .HasForeignKey<Event>(e => e.HallId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(h => h.Seats)
            .WithOne(s => s.Hall)
            .HasForeignKey(s => s.HallId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}