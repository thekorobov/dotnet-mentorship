using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.Property(p => p.Amount)
            .HasColumnType("money")
            .IsRequired();
        
        builder.Property(p => p.Timestamp)
            .IsRequired();
        
        builder.HasOne(p => p.Ticket)
            .WithOne(t => t.Payment)
            .HasForeignKey<Payment>(p => p.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}