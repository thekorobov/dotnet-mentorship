using AutoMapper;
using TicketFlow.Application.Common.Mappings;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Queries.GetAllVerificationCodes;

public record VerificationCodeVm : IMapWith<VerificationCode>
{
    public string VerificationCodeId { get; set; }
    public string UserId { get; set; }
    public string VerificationToken { get; set; }
    public DateTime? VerifiedAt { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<VerificationCode, VerificationCodeVm>()
            .ForMember(verificationCodeVm => verificationCodeVm.UserId,
                opt => opt.MapFrom(
                    verificationCode => verificationCode.UserId))
            .ForMember(verificationCodeVm => verificationCodeVm.VerificationCodeId,
                opt => opt.MapFrom(
                    verificationCode => verificationCode.Id))
            .ForMember(verificationCodeVm => verificationCodeVm.VerificationToken,
                opt => opt.MapFrom(
                    verificationCode => verificationCode.VerificationToken))
            .ForMember(verificationCodeVm => verificationCodeVm.VerifiedAt,
                opt => opt.MapFrom(
                    verificationCode => verificationCode.VerifiedAt));
    }
}