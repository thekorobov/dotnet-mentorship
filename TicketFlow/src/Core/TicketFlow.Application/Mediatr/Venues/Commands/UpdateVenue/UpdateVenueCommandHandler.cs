using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Venues.Commands.UpdateVenue;

public class UpdateVenueCommandHandler : IRequestHandler<UpdateVenueCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateVenueCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateVenueCommand command, CancellationToken cancellationToken = default)
    {
        var venue = await _unitOfWork.Venues.GetAsync(new VenueFilter { Id = command.Id }, cancellationToken);
        if (venue == null)
        {
            throw new EntityNotFoundException(nameof(Venue), command.Id);
        }
        
        var user = await _unitOfWork.Users.GetAsync(new UserFilter { Id = command.UserId }, cancellationToken);
        if (venue.UserId != command.UserId && user.Role != UserRole.Admin.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to update this venue.");
        }

        var updatedVenue = new Venue();
        _mapper.Map(command, updatedVenue);
        
        await _unitOfWork.Venues.UpdateAsync(updatedVenue, cancellationToken);
        
        return Unit.Value;
    }
}