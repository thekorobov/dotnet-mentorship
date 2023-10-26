using MediatR;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Commands.VerifyVerificationCode;

public record VerifyVerificationCodeCommand : IRequest
{
    public string VerificationToken { get; set; }
}