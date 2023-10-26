using AutoMapper;
using Reminders.BLL.DTO;
using Reminders.BLL.DTO.Exceptions;
using Reminders.BLL.Interfaces;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.Users.Queries.GetUserById;

public class GetUserHandler : IQueryHandler<GetUserQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDto> HandleAsync(GetUserQuery query)
    {
        if (string.IsNullOrEmpty(query.Id.ToString()) && string.IsNullOrEmpty(query.Email))
        {
            throw new ArgumentException("Id or email must be provided.");
        }
        
        User user = null;
        
        if (query.Id.HasValue)
        {
            user = await _unitOfWork.Users.GetAsync(new UserFilter { Id = query.Id });
        }
        else if (!string.IsNullOrEmpty(query.Email))
        {
            user = await _unitOfWork.Users.GetAsync(new UserFilter { Email = query.Email });
        }
        
        if (user == null && query.AuthProviderType != AuthProviderType.GoogleAuth)
        {
            var identifier = query.Id.HasValue ? query.Id.Value.ToString() : query.Email;
            throw new EntityNotFoundException(nameof(User), identifier);
        }
        
        return _mapper.Map<UserDto>(user);
    }
}