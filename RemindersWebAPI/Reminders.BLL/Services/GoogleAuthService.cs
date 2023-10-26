using Reminders.BLL.CQS.Users.Commands.CreateUser;
using Reminders.BLL.CQS.Users.Queries.GetAuthToken;
using Reminders.BLL.CQS.Users.Queries.GetUserById;
using Reminders.BLL.CQS.VerificationCodes.Commands.CreateVerificationCode;
using Reminders.BLL.CQS.VerificationCodes.Commands.VerifyVerificationCode;
using Reminders.BLL.CQS.VerificationCodes.Queries.GetVerificationCode;
using Reminders.BLL.DTO;
using Reminders.BLL.Interfaces;
using Reminders.DAL.Entities.Filtres;

namespace Reminders.BLL.Services;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly IMediator _mediator;

    public GoogleAuthService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<string> HandleGoogleLoginAsync(string email, string name)
    {
        var query = new GetUserQuery()
        {
            Email = email,
            AuthProviderType = AuthProviderType.GoogleAuth
        };
        
        var existingUser = await _mediator.SendQueryAsync<GetUserQuery, UserDto>(query);

        if (existingUser == null) 
        {
            var id = await CreateUserAsync(email, name);
            await CreateAndVerifyVerificationCodeAsync(id);
        }

        return await GenerateTokenAsync(email);
    }

    private async Task<int> CreateUserAsync(string email, string name)
    {
        var command = new CreateUserCommand()
        {
            Email = email,
            UserName = name,
            AuthProviderType = AuthProviderType.GoogleAuth
        };

        return await _mediator.SendCommandAsync<CreateUserCommand, int>(command);
    }

    private async Task CreateAndVerifyVerificationCodeAsync(int userId)
    {
        var createVerificationCodeCommand = new CreateVerificationCodeCommand()
        {
            UserId = userId
        };

        await _mediator.SendCommandAsync<CreateVerificationCodeCommand>(createVerificationCodeCommand);

        var getVerificationCodeCommand = new GetVerificationCodeQuery()
        {
            UserId = userId
        };

        var verificationToken = await _mediator.SendQueryAsync<GetVerificationCodeQuery, string>(getVerificationCodeCommand);

        var verifyVerificationCodeCommand = new VerifyVerificationCodeCommand()
        {
            VerificationToken = verificationToken
        };

        await _mediator.SendCommandAsync<VerifyVerificationCodeCommand>(verifyVerificationCodeCommand);
    }

    private async Task<string> GenerateTokenAsync(string email)
    {
        var queryToken = new GetAuthTokenQuery()
        {
            Email = email,
            AuthProviderType = AuthProviderType.GoogleAuth
        };

        return await _mediator.SendQueryAsync<GetAuthTokenQuery, string>(queryToken);
    }
}