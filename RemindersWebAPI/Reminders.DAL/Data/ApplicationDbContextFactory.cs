using Microsoft.EntityFrameworkCore;

namespace Reminders.DAL.Data;

public class ApplicationDbContextFactory : DesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContextFactory() : base("AZURE_POSTGRESQL_CONNECTIONSTRING") {}
  
    protected override ApplicationDbContext CreateDbContext(DbContextOptions<ApplicationDbContext> options)
        => new ApplicationDbContext(options);
}