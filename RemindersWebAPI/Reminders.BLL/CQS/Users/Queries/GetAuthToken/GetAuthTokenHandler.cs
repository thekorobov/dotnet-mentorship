using Microsoft.AspNetCore.Identity;
using Reminders.BLL.DTO.Exceptions;
using Reminders.BLL.Interfaces;
using Reminders.BLL.Utils;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.Users.Queries.GetAuthToken;

public class GetAuthTokenHandler : IQueryHandler<GetAuthTokenQuery, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly UserManager<User> _userManager;

    public GetAuthTokenHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userManager = userManager;
    }
    
    public async Task<string> HandleAsync(GetAuthTokenQuery query)
    {
        var userFilter = new UserFilter { Email = query.Email };
        var user = await _unitOfWork.Users.GetAsync(userFilter);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User does not exist.");
        }

        var userVerificationFilter = new VerificationCodeFilter { UserId = user.Id };
        var userVerification = await _unitOfWork.VerificationCodes.GetAsync(userVerificationFilter);
        
        if (userVerification == null || userVerification.VerifiedAt == null)
        {
            throw new UserNotVerifiedException("User does not verified!.");
        }
        
        if (query.AuthProviderType == AuthProviderType.SimpleAuth)
        {
            if (!(await _userManager.CheckPasswordAsync(user, query.Password)))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
        }

        return _jwtTokenGenerator.GenerateToken(user.Id, user.Email, user.Role);
    }
}