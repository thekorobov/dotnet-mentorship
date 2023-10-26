using AutoMapper;
using TicketFlow.Application.Common.Mappings;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Commands.VerifyVerificationCode;

public record VerifyVerificationCodeDto : IMapWith<VerifyVerificationCodeCommand>
{
    public string VerificationToken { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<VerifyVerificationCodeDto, VerifyVerificationCodeCommand>()
            .ForMember(dest => dest.VerificationToken, 
                opt => opt.MapFrom(src => src.VerificationToken));
    }
}