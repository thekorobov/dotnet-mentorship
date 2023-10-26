using AutoMapper;
using TicketFlow.Application.Common.Mappings;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Queries.GetVerificationCode;

public record GetVerificationCodeVm : IMapWith<GetVerificationCodeQuery>
{
    public string? VerificationCodeId { get; set; }
    public string UserId { get; set; }
    public string VerificationToken { get; set; }
    public DateTime? VerifiedAt { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<VerificationCode, GetVerificationCodeVm>()
            .ForMember(getVerificationCodeVm => getVerificationCodeVm.UserId,
                opt => opt.MapFrom(
                    verificationCode => verificationCode.UserId))
            .ForMember(getVerificationCodeVm => getVerificationCodeVm.VerificationCodeId,
                opt => opt.MapFrom(
                    verificationCode => verificationCode.Id))
            .ForMember(getVerificationCodeVm => getVerificationCodeVm.VerificationToken,
                opt => opt.MapFrom(
                    verificationCode => verificationCode.VerificationToken))
            .ForMember(getVerificationCodeVm => getVerificationCodeVm.VerifiedAt,
                opt => opt.MapFrom(
                    verificationCode => verificationCode.VerifiedAt));
    }
}