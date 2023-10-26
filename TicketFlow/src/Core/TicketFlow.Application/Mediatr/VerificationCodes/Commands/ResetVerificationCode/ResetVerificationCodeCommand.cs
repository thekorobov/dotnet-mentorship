using MediatR;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Commands.ResetVerificationCode;

public record ResetVerificationCodeCommand : IRequest
{
    public string UserId { get; set; }
    public string CurrentUserId { get; set; } 
}