using MediatR;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Commands.CreateVerificationCode;

public record CreateVerificationCodeCommand : IRequest
{
    public string UserId { get; set; }
}