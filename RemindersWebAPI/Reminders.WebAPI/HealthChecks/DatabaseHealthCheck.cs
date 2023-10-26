using Microsoft.Extensions.Diagnostics.HealthChecks;
using Reminders.DAL.Interfaces;

namespace Reminders.WebAPI.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(IUnitOfWork unitOfWork, ILogger<DatabaseHealthCheck> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var reminders = await _unitOfWork.Reminders.GetAllAsync();
            return HealthCheckResult.Healthy(reminders.Any() ? "Database connection is OK" : "No reminders found in the database");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database connection ERROR");
            return HealthCheckResult.Unhealthy("Database connection ERROR");
        }
    }
}