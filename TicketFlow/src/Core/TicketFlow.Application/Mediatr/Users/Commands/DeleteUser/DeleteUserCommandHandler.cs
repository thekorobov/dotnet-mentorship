using MediatR;
using TicketFlow.Application.Mediatr.Users.Queries.GetUser;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    
    public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new GetUserQuery { Id = command.CurrentUserId }, cancellationToken);
        
        await _unitOfWork.Users.DeleteAsync(command.Id, cancellationToken);
        
        return Unit.Value;
    }
}