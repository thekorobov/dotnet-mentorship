using FluentValidation;
using Reminders.BLL.Interfaces;
using Reminders.BLL.Utils;

namespace Reminders.BLL.CQS.ValidationDecorators;

public class CommandResultValidationDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : IValidationRequiredCommand
{
    private readonly ICommandHandler<TCommand, TResult> _commandHandler;
    private readonly IValidator<TCommand> _validator;

    public CommandResultValidationDecorator(ICommandHandler<TCommand, TResult> commandHandler, IValidator<TCommand> validator)
    {
        _commandHandler = commandHandler;
        _validator = validator;
    }

    public async Task<TResult> HandleAsync(TCommand command)
    {
        await ValidationHelper.ValidateCommandAsync(command, _validator);
        return await _commandHandler.HandleAsync(command);
    }
}