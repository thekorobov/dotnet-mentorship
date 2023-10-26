using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketFlow.Application.Mediatr.Halls.Queries.QueryHalls;
using TicketFlow.Application.Mediatr.Seat.Queries.QuerySeats;
using TicketFlow.Application.Mediatr.Users.Queries.QueryUsers;
using TicketFlow.Application.Mediatr.Venues.Queries.QueryVenues;

namespace TicketFlow.WebApi.Controllers;

[Route("api/search")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly IMediator _mediator;

    public SearchController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [HttpGet("users")]
    public async Task<IActionResult> SearchUsersAsync([FromQuery] QueryUsersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Owner, Admin")]
    [HttpGet("venues")]
    public async Task<IActionResult> SearchVenuesAsync([FromQuery] QueryVenuesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Owner, Admin")]
    [HttpGet("halls")]
    public async Task<IActionResult> SearchHallsAsync([FromQuery] QueryHallsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("seats")]
    public async Task<IActionResult> SearchSeatsAsync([FromQuery] QuerySeatsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}