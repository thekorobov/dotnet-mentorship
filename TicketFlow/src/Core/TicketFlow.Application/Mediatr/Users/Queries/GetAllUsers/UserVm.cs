using AutoMapper;
using TicketFlow.Application.Common.Mappings;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Enums.Users;

namespace TicketFlow.Application.Mediatr.Users.Queries.GetAllUsers;

public record UserVm : IMapWith<User>
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Surname { get; set; }
    public string Forename { get; set; }
    public string Role { get; set; }
    public AuthProviderType AuthProviderType { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserVm>()
            .ForMember(getAllUsersVm => getAllUsersVm.UserName,
                opt => opt.MapFrom(user => user.UserName))
            .ForMember(getAllUsersVm => getAllUsersVm.Email,
                opt => opt.MapFrom(user => user.Email))
            .ForMember(getAllUsersVm => getAllUsersVm.Surname,
                opt => opt.MapFrom(user => user.Surname))
            .ForMember(getAllUsersVm => getAllUsersVm.Forename,
                opt => opt.MapFrom(user => user.Forename))
            .ForMember(getAllUsersVm => getAllUsersVm.Role,
                opt => opt.MapFrom(user => user.Role))
            .ForMember(getAllUsersVm => getAllUsersVm.AuthProviderType,
                opt => opt.MapFrom(user => user.AuthProviderType));
    }
}