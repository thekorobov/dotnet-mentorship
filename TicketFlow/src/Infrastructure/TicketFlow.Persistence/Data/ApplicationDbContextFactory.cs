using Microsoft.EntityFrameworkCore;

namespace TicketFlow.Persistence.Data;

public class ApplicationDbContextFactory : DesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContextFactory() : base("DefaultConnection") {}
  
    protected override ApplicationDbContext CreateDbContext(DbContextOptions<ApplicationDbContext> options)
        => new ApplicationDbContext(options);
}