using MediatR;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Queries.GetAllVerificationCodes;

public record GetAllVerificationCodesQuery : IRequest<GetAllVerificationCodesVm>
{

}