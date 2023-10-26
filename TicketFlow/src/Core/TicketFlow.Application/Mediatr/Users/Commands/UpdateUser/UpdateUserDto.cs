using AutoMapper;
using TicketFlow.Application.Common.Mappings;

namespace TicketFlow.Application.Mediatr.Users.Commands.UpdateUser;

public record UpdateUserDto : IMapWith<UpdateUserCommand>
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Surname { get; set; }
    public string Forename { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateUserDto, UpdateUserCommand>()
            .ForMember(dest => dest.Id, 
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, 
                opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, 
                opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, 
                opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Surname, 
                opt => opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Forename, 
                opt => opt.MapFrom(src => src.Forename));
    }
}