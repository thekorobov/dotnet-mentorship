using System.Reflection;
using FluentValidation;
using Reminders.BLL.CQS.ValidationDecorators;
using Reminders.BLL.Interfaces;

namespace Reminders.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterGenericTypes(this IServiceCollection services, Assembly assembly, string typeSuffix)
    {
        var types = assembly.GetTypes()
            .Where(t => t.Name.EndsWith(typeSuffix) && !t.IsAbstract && !t.IsInterface)
            .Select(a => new { assignedType = a, serviceTypes = a.GetInterfaces().ToList() })
            .ToList();
        types.ForEach(typesToRegister =>
        {
            typesToRegister.serviceTypes.ForEach(typeToRegister => services.AddTransient(typeToRegister, typesToRegister.assignedType));
        });
    }
    
    public static void AddCommandWithValidation<TCommand, THandler, TValidator>(this IServiceCollection services)
        where TCommand : IValidationRequiredCommand
        where THandler : class, ICommandHandler<TCommand>
        where TValidator : class, IValidator<TCommand>
    {
        services.AddTransient<THandler>();
        services.AddTransient<TValidator>();
        services.AddTransient<ICommandHandler<TCommand>>(provider =>
        {
            var handler = provider.GetRequiredService<THandler>();
            var validator = provider.GetRequiredService<TValidator>();
            return new CommandValidationDecorator<TCommand>(handler, validator);
        });
    }
    
    public static void AddCommandWithValidationAndResult<TCommand, TResult, THandler, TValidator>(this IServiceCollection services)
        where TCommand : IValidationRequiredCommand
        where THandler : class, ICommandHandler<TCommand, TResult>
        where TValidator : class, IValidator<TCommand>
    {
        services.AddTransient<THandler>();
        services.AddTransient<TValidator>();
        services.AddTransient<ICommandHandler<TCommand, TResult>>(provider =>
        {
            var handler = provider.GetRequiredService<THandler>();
            var validator = provider.GetRequiredService<TValidator>();
            return new CommandResultValidationDecorator<TCommand, TResult>(handler, validator);
        });
    }
}