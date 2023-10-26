using Reminders.BLL.DTO.Exceptions;
using Reminders.BLL.Interfaces;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.Reminders.Commands.DeleteReminder;

public class DeleteReminderHandler : ICommandHandler<DeleteReminderCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReminderHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(DeleteReminderCommand command)
    {
        var filter = new ReminderFilter { Id = command.Id };
        var reminder = await _unitOfWork.Reminders.GetAsync(filter);

        if (reminder != null)
        {
            if (reminder.UserId != command.UserId)
            {
                throw new UnauthorizedAccessException("Access to delete the reminder is denied.");
            }
            
            await _unitOfWork.Reminders.DeleteAsync(command.Id);
        }
        else
        {
            throw new EntityNotFoundException(nameof(Reminder), command.Id);
        }
    }
}