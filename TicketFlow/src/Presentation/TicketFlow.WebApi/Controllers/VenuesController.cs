using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketFlow.Application.Mediatr.Venues.Commands.CreateVenue;
using TicketFlow.Application.Mediatr.Venues.Commands.DeleteVenue;
using TicketFlow.Application.Mediatr.Venues.Commands.UpdateVenue;
using TicketFlow.Application.Mediatr.Venues.Queries.GetAllVenues;
using TicketFlow.Application.Mediatr.Venues.Queries.GetVenue;

namespace TicketFlow.WebApi.Controllers;

[Authorize(AuthenticationSchemes = "Bearer", Roles = "Owner, Admin")]
[Route("api/venues")]
[ApiController]
public class VenuesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public VenuesController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateVenueAsync([FromBody] CreateVenueDto createVenueDto)
    {
        var command = _mapper.Map<CreateVenueCommand>(createVenueDto);
        command.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        command.Role = User.FindFirst(ClaimTypes.Role)?.Value!;
        
        var venueId = await _mediator.Send(command);

        return Ok(new { venueId });
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateVenueAsync([FromBody] UpdateVenueDto updateVenueDto)
    {
        var command = _mapper.Map<UpdateVenueCommand>(updateVenueDto);
        command.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        
        await _mediator.Send(command);

        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVenueAsync(string id)
    {
        await _mediator.Send(new DeleteVenueCommand
        {
            Id = id, 
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!
        });
        return Ok();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVenueAsync(string id, [FromQuery] bool includeHalls = false, [FromQuery] bool includeSeats = false)
    {
        var result = await _mediator.Send(new GetVenueQuery
        {
            Id = id, 
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            IncludeHalls = includeHalls, 
            IncludeSeats = includeSeats
        });
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetVenuesAsync([FromQuery] bool includeHalls = false, [FromQuery] bool includeSeats = false)
    {
        var result = await _mediator.Send(new GetAllVenuesQuery
        {
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value, 
            IncludeHalls = includeHalls, IncludeSeats = includeSeats
        });
        return Ok(result);
    }
}