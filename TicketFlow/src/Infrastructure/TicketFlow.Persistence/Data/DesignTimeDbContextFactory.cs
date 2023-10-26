using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TicketFlow.Persistence.Data;

public abstract class DesignTimeDbContextFactory<T> : IDesignTimeDbContextFactory<T>
    where T : DbContext
{
    private IConfiguration Configuration { get; }
    private string ConfigKey { get; }

    public DesignTimeDbContextFactory(string configKey)
    {
        this.ConfigKey = configKey ?? throw new ArgumentNullException(nameof(configKey));
        var cb = new ConfigurationBuilder();
        AddConfigurationSources(cb, Assembly.GetCallingAssembly());
        Configuration = cb.Build();
    }

    protected virtual void AddConfigurationSources(ConfigurationBuilder builder, Assembly asm)
    {
        builder.AddUserSecrets(asm).AddEnvironmentVariables();
    }

    public T CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<T>();
        builder.UseNpgsql(Configuration.GetConnectionString(ConfigKey));
        return CreateDbContext(builder.Options);
    }

    protected abstract T CreateDbContext(DbContextOptions<T> options);
}