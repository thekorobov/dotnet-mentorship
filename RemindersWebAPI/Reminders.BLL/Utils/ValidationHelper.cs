using FluentValidation;
using FluentValidation.Results;

namespace Reminders.BLL.Utils;

public static class ValidationHelper
{
    public static async Task ValidateCommandAsync<T>(T command, IValidator<T> validator)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }
}