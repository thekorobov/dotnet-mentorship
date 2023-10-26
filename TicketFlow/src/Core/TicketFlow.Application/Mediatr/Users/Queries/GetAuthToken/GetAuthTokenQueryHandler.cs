using MediatR;
using Microsoft.AspNetCore.Identity;
using TicketFlow.Application.Common.Exceptions.VerificationCodes;
using TicketFlow.Application.Common.Interfaces;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Users.Queries.GetAuthToken;

public class GetAuthTokenQueryHandler : IRequestHandler<GetAuthTokenQuery, GetAuthTokenVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly UserManager<User> _userManager;

    public GetAuthTokenQueryHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator,
        UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userManager = userManager;
    }

    public async Task<GetAuthTokenVm> Handle(GetAuthTokenQuery query, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetAsync(new UserFilter { Email = query.Email }, cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User does not exist.");
        }

        var userVerification = await _unitOfWork.VerificationCodes.GetAsync
            (new VerificationCodeFilter { UserId = user.Id }, cancellationToken);

        if (userVerification == null || userVerification.VerifiedAt == null)
        {
            throw new UserNotVerifiedException("User does not verified!");
        }

        if (query.AuthProviderType == AuthProviderType.SimpleAuth)
        {
            if (!(await _userManager.CheckPasswordAsync(user, query.Password)))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email!, user.Role);
        return new GetAuthTokenVm { Token = token };
    }
}