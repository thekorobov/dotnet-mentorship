using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Persistence.Configurations;

public class VerificationCodeConfiguration : IEntityTypeConfiguration<VerificationCode>
{
    public void Configure(EntityTypeBuilder<VerificationCode> builder)
    {
        builder.HasKey(vc => vc.Id);
        
        builder.Property(vc => vc.Id)
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.HasOne(u => u.User)
            .WithOne(u => u.VerificationCode)
            .HasForeignKey<VerificationCode>(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}