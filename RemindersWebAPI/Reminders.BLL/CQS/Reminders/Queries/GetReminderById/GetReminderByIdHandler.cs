using AutoMapper;
using Reminders.BLL.DTO;
using Reminders.BLL.DTO.Exceptions;
using Reminders.BLL.Interfaces;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.Reminders.Queries.GetReminderById;

public class GetReminderByIdHandler : IQueryHandler<GetReminderByIdQuery, ReminderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetReminderByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReminderDto> HandleAsync(GetReminderByIdQuery query)
    {
        var filter = new ReminderFilter { Id = query.Id };
        var reminder = await _unitOfWork.Reminders.GetAsync(filter);

        if (reminder != null)
        {
            if (reminder.UserId != query.UserId)
            {
                throw new UnauthorizedAccessException("Access to the reminder is denied.");
            }
            
            return _mapper.Map<ReminderDto>(reminder);
        }
        else
        {
            throw new EntityNotFoundException(nameof(Reminder), query.Id);
        }
    }
}