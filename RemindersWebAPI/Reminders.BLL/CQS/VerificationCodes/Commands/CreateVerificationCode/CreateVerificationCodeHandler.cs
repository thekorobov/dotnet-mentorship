using AutoMapper;
using Reminders.BLL.DTO.Exceptions;
using Reminders.BLL.Interfaces;
using Reminders.BLL.Utils;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.VerificationCodes.Commands.CreateVerificationCode;

public class CreateVerificationCodeHandler : ICommandHandler<CreateVerificationCodeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public CreateVerificationCodeHandler(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task HandleAsync(CreateVerificationCodeCommand command)
    {
        var existingUser = await _unitOfWork.Users.GetAsync(new UserFilter { Id = command.UserId });
        if (existingUser == null)
        {
            throw new EntityNotFoundException(nameof(User));
        }
    
        var existingUserVerification = await _unitOfWork.VerificationCodes.GetAsync(new VerificationCodeFilter 
            { UserId = command.UserId });
        if (existingUserVerification != null)
        {
            if (existingUserVerification.VerifiedAt.HasValue)
            {
                throw new UserAlreadyVerifiedException("User is already verified.");
            }
        }
        
        var userVerification = _mapper.Map<CreateVerificationCodeCommand, VerificationCode>(command);
        userVerification.UserId = command.UserId;
        userVerification.VerifiedAt = null;
        userVerification.VerificationToken = VerificationCodeGenerateHelper.GenerateVerificationCode();
        
        await _unitOfWork.VerificationCodes.CreateAsync(userVerification);

        if (existingUser.AuthProviderType == AuthProviderType.SimpleAuth)
        {
            var sendUserVerification  = await _unitOfWork.VerificationCodes.GetAsync(new VerificationCodeFilter 
                { UserId = command.UserId });
        
            await _emailService.SendEmailAsync(
                existingUser.Email,
                "Your verification code",
                $"Your verification code is <strong>{sendUserVerification.VerificationToken}</strong>"
            );   
        }
    }
}