using MediatR;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Queries.GetVerificationCode;

public record GetVerificationCodeQuery : IRequest<GetVerificationCodeVm>
{
    public string? VerificationCodeId { get; set; }
    public string? UserId { get; set; }
    public string? VerificationToken { get; set; }
}