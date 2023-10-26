using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketFlow.Application.Mediatr.Seat.Commands.CreateSeat;
using TicketFlow.Application.Mediatr.Seat.Commands.DeleteSeat;
using TicketFlow.Application.Mediatr.Seat.Commands.UpdateSeat;
using TicketFlow.Application.Mediatr.Seat.Queries.GetAllSeats;
using TicketFlow.Application.Mediatr.Seat.Queries.GetSeat;
using TicketFlow.Domain.Enums;

namespace TicketFlow.WebApi.Controllers;

[Authorize(AuthenticationSchemes = "Bearer", Roles = "Owner, Admin")]
[Route("api/seats")]
[ApiController]
public class SeatsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public SeatsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateSeatAsync([FromBody] CreateSeatDto createSeatDto)
    {
        var command = _mapper.Map<CreateSeatCommand>(createSeatDto);
        command.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        command.Role = User.FindFirst(ClaimTypes.Role)?.Value!;
        
        var seatId = await _mediator.Send(command);

        return Ok(new { venueId = seatId });
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateSeatAsync([FromBody] UpdateSeatDto updateSeatDto)
    {
        var command = _mapper.Map<UpdateSeatCommand>(updateSeatDto);
        command.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        
        await _mediator.Send(command);

        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSeatAsync(string id)
    {
        await _mediator.Send(new DeleteSeatCommand
        {
            Id = id, 
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!
        });
        return Ok();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSeatAsync(string id)
    {
        var result = await _mediator.Send(new GetSeatQuery { Id = id });
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetSeatsAsync([FromQuery] string? venueId, [FromQuery] string? hallId, [FromQuery] SeatStatus? status)
    {
        var result = await _mediator.Send(new GetAllSeatsQuery()
        {
            VenueId = venueId,
            HallId = hallId,
            Status = status
        });
        return Ok(result);
    }
}