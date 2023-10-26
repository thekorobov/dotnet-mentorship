using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.Property(e => e.Title)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(e => e.Description)
            .HasMaxLength(2000);
        
        builder.Property(e => e.Date)
            .IsRequired();
        
        builder.Property(e => e.Duration)
            .IsRequired();
        
        builder.HasOne(e => e.Venue)
            .WithMany(v => v.Events)
            .HasForeignKey(e => e.VenueId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Hall)
            .WithOne(h => h.Event)
            .HasForeignKey<Event>(e => e.HallId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Tickets)
            .WithOne(t => t.Event)
            .HasForeignKey(t => t.EventId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(e => e.Title).HasDatabaseName("IX_Events_Title");
    }
}