using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketFlow.Application.Mediatr.Users.Commands.CreateUser;
using TicketFlow.Application.Mediatr.Users.Commands.DeleteUser;
using TicketFlow.Application.Mediatr.Users.Commands.UpdateUser;
using TicketFlow.Application.Mediatr.Users.Queries.GetAllUsers;
using TicketFlow.Application.Mediatr.Users.Queries.GetAuthToken;
using TicketFlow.Application.Mediatr.Users.Queries.GetUser;
using TicketFlow.Domain.Enums.Users;

namespace TicketFlow.WebApi.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;
    private readonly IMapper _mapper;

    public UsersController(IMediator mediator, ILogger<UsersController> logger, IMapper mapper)
    {
        _mediator = mediator;
        _logger = logger;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost("sign-up")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto createUserDto)
    {
        var command = _mapper.Map<CreateUserCommand>(createUserDto);
        command.AuthProviderType = AuthProviderType.SimpleAuth;

        var userId = await _mediator.Send(command);

        return Ok(new { userId });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] GetAuthTokenDto getAuthTokenDto)
    {
        var query = new GetUserQuery
        {
            Email = getAuthTokenDto.Email
        };
        await _mediator.Send(query);

        var queryToken = _mapper.Map<GetAuthTokenQuery>(getAuthTokenDto);
        queryToken.AuthProviderType = AuthProviderType.SimpleAuth;

        var token = await _mediator.Send(queryToken);

        return Ok(token);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto updateUserDto)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var command = _mapper.Map<UpdateUserCommand>(updateUserDto);
        command.CurrentUserId = currentUserId!;
        await _mediator.Send(command);

        return Ok();
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(string id)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await _mediator.Send(new DeleteUserCommand { Id = id, CurrentUserId = currentUserId! });
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserAsync(string id)
    {
        var result = await _mediator.Send(new GetUserQuery { Id = id } );
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsersAsync()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }
}