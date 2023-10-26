using AutoMapper;
using Reminders.BLL.DTO;
using Reminders.BLL.Interfaces;
using Reminders.DAL.Entities;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.Users.Queries.GetAllUsers;

public class GetAllUsersHandler : IQueryHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllUsersHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<UserDto>> HandleAsync(GetAllUsersQuery query)
    {
        var users = await _unitOfWork.Users.GetAllAsync() ?? new List<User>();

        var usersDto = _mapper.Map<List<User>, List<UserDto>>(users.ToList()) ?? new List<UserDto>();

        return usersDto.Any() ? usersDto : new List<UserDto>();
    }
}