using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
            
        builder.Property(n => n.Id)
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);  

        builder.Property(n => n.Message).IsRequired();
        builder.Property(n => n.Type).IsRequired();
        builder.Property(n => n.Status).IsRequired();
        builder.Property(n => n.Timestamp).IsRequired();
    }
}