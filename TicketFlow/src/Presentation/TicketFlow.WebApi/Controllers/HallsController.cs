using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketFlow.Application.Mediatr.Halls.Commands.CreateHall;
using TicketFlow.Application.Mediatr.Halls.Commands.DeleteHall;
using TicketFlow.Application.Mediatr.Halls.Commands.UpdateHall;
using TicketFlow.Application.Mediatr.Halls.Queries.GetAllHall;
using TicketFlow.Application.Mediatr.Halls.Queries.GetHall;

namespace TicketFlow.WebApi.Controllers;

[Authorize(AuthenticationSchemes = "Bearer", Roles = "Owner, Admin")]
[Route("api/halls")]
[ApiController]
public class HallsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public HallsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateHallAsync([FromBody] CreateHallDto createHallDto)
    {
        var command = _mapper.Map<CreateHallCommand>(createHallDto);
        command.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        command.Role = User.FindFirst(ClaimTypes.Role)?.Value!;
        
        var hallId = await _mediator.Send(command);

        return Ok(new { hallId });
    }
     
    [HttpPut]
    public async Task<IActionResult> UpdateHallAsync([FromBody] UpdateHallDto updateHallDto)
    {
        var command = _mapper.Map<UpdateHallCommand>(updateHallDto);
        command.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        
        await _mediator.Send(command);

        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHallAsync(string id)
    {
        await _mediator.Send(new DeleteHallCommand
        {
            Id = id, 
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!
        });
        return Ok();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetHallAsync(string id, [FromQuery] bool includeSeats = false)
    {
        var result = await _mediator.Send(new GetHallQuery
        {
            Id = id, 
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value, 
            IncludeSeats = includeSeats
        });
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetHallsAsync([FromQuery] string? venueId, [FromQuery] bool includeSeats = false)
    {
        var result = await _mediator.Send(new GetAllHallsQuery()
        {
            VenueId = venueId,
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value, 
            IncludeSeats = includeSeats
        });
        return Ok(result);
    }
}