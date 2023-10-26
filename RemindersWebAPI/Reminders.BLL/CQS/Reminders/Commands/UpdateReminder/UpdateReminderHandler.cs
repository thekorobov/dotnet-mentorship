using AutoMapper;
using Reminders.BLL.DTO.Exceptions;
using Reminders.BLL.Interfaces;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.Reminders.Commands.UpdateReminder;

public class UpdateReminderHandler : ICommandHandler<UpdateReminderCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateReminderHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task HandleAsync(UpdateReminderCommand command)
    {
        var filter = new ReminderFilter { Id = command.Id };
        var reminder = await _unitOfWork.Reminders.GetAsync(filter);

        if (reminder == null)
        {
            throw new EntityNotFoundException(nameof(Reminder));
        }

        if (reminder.UserId != command.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this reminder.");
        }
    
        var updatedReminder = _mapper.Map<Reminder>(command);
        await _unitOfWork.Reminders.UpdateAsync(updatedReminder);
    }
}