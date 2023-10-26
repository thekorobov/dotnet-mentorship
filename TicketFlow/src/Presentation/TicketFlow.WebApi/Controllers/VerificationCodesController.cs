using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketFlow.Application.Mediatr.VerificationCodes.Commands.VerifyVerificationCode;
using TicketFlow.Application.Mediatr.VerificationCodes.Queries.GetAllVerificationCodes;
using TicketFlow.Application.Mediatr.VerificationCodes.Queries.GetVerificationCode;

namespace TicketFlow.WebApi.Controllers;

[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
[Route("api/verification-codes")]
[ApiController]
public class VerificationCodesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public VerificationCodesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [AllowAnonymous]
    [HttpPatch("{token}/status")]
    public async Task<IActionResult> VerifyUserAsync(string token)
    {
        var verifyCommand = _mapper.Map<VerifyVerificationCodeCommand>(
            new VerifyVerificationCodeDto { VerificationToken = token });
        await _mediator.Send(verifyCommand);
        return Ok();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVerificationCodeAsync(string id)
    {
        var result = await _mediator.Send(new GetVerificationCodeQuery { VerificationCodeId = id });
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetVerificationCodesAsync()
    {
        var result = await _mediator.Send(new GetAllVerificationCodesQuery());
        return Ok(result);
    }
}