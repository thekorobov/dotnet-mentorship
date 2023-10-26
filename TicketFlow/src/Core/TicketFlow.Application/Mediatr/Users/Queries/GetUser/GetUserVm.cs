using AutoMapper;
using TicketFlow.Application.Common.Mappings;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Enums.Users;

namespace TicketFlow.Application.Mediatr.Users.Queries.GetUser;

public class GetUserVm : IMapWith<User>
{
    public string Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public AuthProviderType AuthProviderType { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, GetUserVm>()
            .ForMember(getAllUsersVm => getAllUsersVm.UserName,
                opt => opt.MapFrom(user => user.UserName))
            .ForMember(getAllUsersVm => getAllUsersVm.Email,
                opt => opt.MapFrom(user => user.Email))
            .ForMember(getAllUsersVm => getAllUsersVm.AuthProviderType,
                opt => opt.MapFrom(user => user.AuthProviderType));
    }
}