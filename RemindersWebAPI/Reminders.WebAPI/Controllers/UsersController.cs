using System.Security.Claims;
using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reminders.BLL.CQS.Users.Commands.CreateUser;
using Reminders.BLL.CQS.Users.Commands.DeleteUser;
using Reminders.BLL.CQS.Users.Commands.ResetUserEmail;
using Reminders.BLL.CQS.Users.Commands.UpdateUser;
using Reminders.BLL.CQS.Users.Queries.GetAllUsers;
using Reminders.BLL.CQS.Users.Queries.GetAuthToken;
using Reminders.BLL.CQS.Users.Queries.GetUserById;
using Reminders.BLL.CQS.VerificationCodes.Commands.CreateVerificationCode;
using Reminders.BLL.CQS.VerificationCodes.Commands.ResetVerificationCode;
using Reminders.BLL.CQS.VerificationCodes.Commands.VerifyVerificationCode;
using Reminders.BLL.DTO;
using Reminders.BLL.Interfaces;
using Reminders.DAL.Entities.Filtres;
using Reminders.WebAPI.Models;

namespace Reminders.WebAPI.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;
    private readonly IMapper _mapper;
    private readonly IGoogleAuthService _googleAuthService;

    public UsersController(IMediator mediator, ILogger<UsersController> logger, IMapper mapper, IGoogleAuthService googleAuthService)
    {
        _mediator = mediator;
        _logger = logger;
        _mapper = mapper;
        _googleAuthService = googleAuthService;
    }
    
    [HttpPost("sign-up")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserCommand command)
    {
        command.AuthProviderType = AuthProviderType.SimpleAuth;
        var id = await _mediator.SendCommandAsync<CreateUserCommand, int>(command);

        var createVerificationCodeCommand = new CreateVerificationCodeCommand()
        {
            UserId = id
        };
        await _mediator.SendCommandAsync<CreateVerificationCodeCommand>(createVerificationCodeCommand);
        
        return Ok( new { id });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] GetAuthTokenQuery loginQuery)
    {
        var query = new GetUserQuery()
        {
            Email = loginQuery.Email
        };
        await _mediator.SendQueryAsync<GetUserQuery, UserDto>(query);

        var queryToken = new GetAuthTokenQuery()
        {
            Email = loginQuery.Email,
            Password = loginQuery.Password,
            AuthProviderType = AuthProviderType.SimpleAuth
        };
        var token = await _mediator.SendQueryAsync<GetAuthTokenQuery, string>(queryToken);
            
        return Ok(new { token });
    }
  
    [HttpPost("verify")]
    public async Task<IActionResult> VerifyUserAsync([FromQuery] VerifyVerificationCodeCommand command)
    {
        await _mediator.SendCommandAsync(command);
        return Ok();
    }
    
    [HttpPost("signin-google")]
    public async Task<IActionResult> GoogleLoginAsync([FromBody] TokenModel model)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(model.Token, new GoogleJsonWebSignature.ValidationSettings());
        var email = payload.Email;
        var name = payload.Name;

        var token = await _googleAuthService.HandleGoogleLoginAsync(email, name);

        return Ok(new { token });
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("reset-email")]
    public async Task<IActionResult> ResetUserEmailAsync([FromBody] ResetUserEmailCommand command)
    {
        if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int currentUserId))
        {
            command.CurrentUserId = currentUserId;
            
            await _mediator.SendCommandAsync(command);
        
            var resetVerificationCodeCommand = new ResetVerificationCodeCommand()
            {
                UserId = command.Id,
                CurrentUserId = currentUserId
            };
            await _mediator.SendCommandAsync<ResetVerificationCodeCommand>(resetVerificationCodeCommand);
        
            return Ok();
        }
        else
        {
            throw new ArgumentException("Invalid user Id");
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("reset-password")]
    public async Task<IActionResult> ResetUserPasswordAsync([FromBody] ResetUserPasswordCommand passwordCommand)
    {
        if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int currentUserId))
        {
            passwordCommand.CurrentUserId = currentUserId;
            await _mediator.SendCommandAsync(passwordCommand);
            return Ok();
        }
        else
        {
            throw new ArgumentException("Invalid user Id");
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            await _mediator.SendCommandAsync(new DeleteUserCommand { Id = id, CurrentUserId = userId });
            return Ok();
        }
        else
        {
            throw new ArgumentException("Invalid user Id");
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("get-user")]
    public async Task<IActionResult> GetUserAsync([FromQuery] GetUserQuery query)
    {
        var result = await _mediator.SendQueryAsync<GetUserQuery, UserDto>(query);
        var mappedResult = _mapper.Map<UserModel>(result);
        return Ok(mappedResult);
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    public async Task<IActionResult> GetUsersAsync()
    {
        var result = await _mediator.SendQueryAsync<GetAllUsersQuery, List<UserDto>>(new GetAllUsersQuery());
        var mappedResult = _mapper.Map<List<UserDto>, List<UserModel>>(result);
        return Ok(mappedResult);
    }
}