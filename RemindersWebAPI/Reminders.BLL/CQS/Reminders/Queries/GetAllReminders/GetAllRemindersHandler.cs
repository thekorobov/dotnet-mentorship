using AutoMapper;
using Reminders.BLL.DTO;
using Reminders.BLL.Interfaces;
using Reminders.DAL.Entities;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.Reminders.Queries.GetAllReminders;

public class GetAllRemindersHandler : IQueryHandler<GetAllRemindersQuery, List<ReminderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllRemindersHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ReminderDto>> HandleAsync(GetAllRemindersQuery query)
    {
        var reminders = await _unitOfWork.Reminders.GetAllAsync();
    
        var filteredReminders = reminders.Where(r => r.UserId == query.UserId).ToList();
    
        var remindersDto = _mapper.Map<List<Reminder>, List<ReminderDto>>(filteredReminders);
    
        return remindersDto?.Any() ?? false ? remindersDto : new List<ReminderDto>();
    }
}