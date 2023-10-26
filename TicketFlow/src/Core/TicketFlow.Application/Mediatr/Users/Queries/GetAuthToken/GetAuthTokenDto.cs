using AutoMapper;
using TicketFlow.Application.Common.Mappings;

namespace TicketFlow.Application.Mediatr.Users.Queries.GetAuthToken;

public record GetAuthTokenDto : IMapWith<GetAuthTokenQuery>
{
    public string Email { get; set; }
    public string Password { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<GetAuthTokenDto, GetAuthTokenQuery>()
            .ForMember(dest => dest.Email, 
                opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, 
                opt => opt.MapFrom(src => src.Password));
    }
}