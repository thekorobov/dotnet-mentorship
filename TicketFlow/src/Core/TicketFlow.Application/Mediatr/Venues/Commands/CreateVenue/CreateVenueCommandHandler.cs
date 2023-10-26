using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Venues.Commands.CreateVenue;

public class CreateVenueCommandHandler : IRequestHandler<CreateVenueCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public CreateVenueCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<string> Handle(CreateVenueCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Role == UserRole.User.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to create venue.");
        }
        
        var venue = _mapper.Map<CreateVenueCommand, Venue>(command);
        
        await _unitOfWork.Venues.CreateAsync(venue, cancellationToken);
        
        return venue.Id;
    }
}