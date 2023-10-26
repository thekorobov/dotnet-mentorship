using FluentValidation;
using Reminders.BLL.Interfaces;
using Reminders.BLL.Utils;

namespace Reminders.BLL.CQS.ValidationDecorators;

public class CommandValidationDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : IValidationRequiredCommand
{
    private readonly ICommandHandler<TCommand> _commandHandler;
    private readonly IValidator<TCommand> _validator;

    public CommandValidationDecorator(ICommandHandler<TCommand> commandHandler, IValidator<TCommand> validator)
    {
        _commandHandler = commandHandler;
        _validator = validator;
    }
    
    public async Task HandleAsync(TCommand command)
    {
        await ValidationHelper.ValidateCommandAsync(command, _validator);
        await _commandHandler.HandleAsync(command);
    }
}