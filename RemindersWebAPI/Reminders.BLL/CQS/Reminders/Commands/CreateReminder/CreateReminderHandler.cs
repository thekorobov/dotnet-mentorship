using AutoMapper;
using Reminders.BLL.Interfaces;
using Reminders.DAL.Entities;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.Reminders.Commands.CreateReminder;

public class CreateReminderHandler : ICommandHandler<CreateReminderCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateReminderHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> HandleAsync(CreateReminderCommand command)
    {
        var reminder = _mapper.Map<Reminder>(command);
        var result = await _unitOfWork.Reminders.CreateAsync(reminder);
        return result;
    }
}