using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TicketFlow.Application.Common.Mappings;
using TicketFlow.Domain.Enums.Users;

namespace TicketFlow.Application.Mediatr.Users.Commands.CreateUser;

public record CreateUserDto : IMapWith<CreateUserCommand>
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    public string Forename { get; set; }
    [Required]
    public RegistrationRole Role { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateUserDto, CreateUserCommand>()
            .ForMember(dest => dest.UserName, 
                opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, 
                opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, 
                opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Surname, 
                opt => opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Forename, 
                opt => opt.MapFrom(src => src.Forename))
            .ForMember(dest => dest.Role, 
                opt => opt.MapFrom(src => src.Role.ToString()));
    }
}