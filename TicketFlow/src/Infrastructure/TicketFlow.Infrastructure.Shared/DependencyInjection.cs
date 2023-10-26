using Microsoft.Extensions.DependencyInjection;
using TicketFlow.Infrastructure.Shared.Services.Email.Abstractions;
using TicketFlow.Infrastructure.Shared.Services.Email.Implementations;

namespace TicketFlow.Infrastructure.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureShared(this IServiceCollection services)
    {
        services.AddSingleton<ISmtpClientWrapper, SmtpClientWrapper>();
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}