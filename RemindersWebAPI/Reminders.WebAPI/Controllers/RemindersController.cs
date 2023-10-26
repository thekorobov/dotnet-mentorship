using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reminders.BLL.CQS.Reminders.Commands.CreateReminder;
using Reminders.BLL.CQS.Reminders.Commands.DeleteReminder;
using Reminders.BLL.CQS.Reminders.Commands.UpdateReminder;
using Reminders.BLL.CQS.Reminders.Queries.GetAllReminders;
using Reminders.BLL.CQS.Reminders.Queries.GetReminderById;
using Reminders.BLL.DTO;
using Reminders.BLL.Interfaces;
using Reminders.WebAPI.Models;

namespace Reminders.WebAPI.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/reminders")]
[ApiController]
public class RemindersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RemindersController> _logger;
    private readonly IMapper _mapper;

    public RemindersController(IMediator mediator, ILogger<RemindersController> logger, IMapper mapper)
    {
        _mediator = mediator;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReminderAsync([FromBody] CreateReminderCommand command)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        command.UserId = Convert.ToInt32(userId);
        
        var id = await _mediator.SendCommandAsync<CreateReminderCommand, int>(command);
        return Ok( new { id });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateReminderAsync([FromBody] UpdateReminderCommand command)
    {
        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        command.UserId = currentUserId;
        
        await _mediator.SendCommandAsync(command);
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteReminderAsync(int id)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (Int32.TryParse(userIdStr, out int userId))
        {
            var command = new DeleteReminderCommand()
            {
                Id = id,
                UserId = userId
            };
        
            await _mediator.SendCommandAsync(command);
            return Ok();
        }
        else
        {
            throw new ArgumentException("Invalid user ID");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetReminderByIdAsync(int id)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (Int32.TryParse(userIdStr, out int userId))
        {
            var query = new GetReminderByIdQuery()
            {
                Id = id,
                UserId = userId
            };
        
            var result = await _mediator.SendQueryAsync<GetReminderByIdQuery, ReminderDto>(query);
            var mappedResult = _mapper.Map<ReminderModel>(result);
            return Ok(mappedResult);
        }
        else
        {
            throw new ArgumentException("Invalid user ID");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetRemindersAsync()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (Int32.TryParse(userIdStr, out int userId))
        {
            var query = new GetAllRemindersQuery()
            {
                UserId = userId
            };
        
            var result = await _mediator.SendQueryAsync<GetAllRemindersQuery, List<ReminderDto>>(query);
            var mappedResult = _mapper.Map<List<ReminderDto>, List<ReminderModel>>(result);
            return Ok(mappedResult);
        }
        else
        {
            throw new ArgumentException("Invalid user ID");
        }
    }
}