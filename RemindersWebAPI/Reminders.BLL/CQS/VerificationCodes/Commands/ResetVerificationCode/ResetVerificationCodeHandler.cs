using AutoMapper;
using Reminders.BLL.DTO.Exceptions;
using Reminders.BLL.Interfaces;
using Reminders.BLL.Utils;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.VerificationCodes.Commands.ResetVerificationCode;

public class ResetVerificationCodeHandler : ICommandHandler<ResetVerificationCodeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public ResetVerificationCodeHandler(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
    }
    
    public async Task HandleAsync(ResetVerificationCodeCommand command)
    {
        if (command.UserId != command.CurrentUserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this user.");
        }
        
        var existingUserVerification  = await _unitOfWork.VerificationCodes.GetAsync(new VerificationCodeFilter 
            { UserId = command.UserId });

        if (existingUserVerification == null)
        {
            throw new EntityNotFoundException(nameof(VerificationCode));
        }

        var userVerification = _mapper.Map<ResetVerificationCodeCommand, VerificationCode>(command);
        userVerification.VerifiedAt = null;
        userVerification.VerificationToken = VerificationCodeGenerateHelper.GenerateVerificationCode();
            
        await _unitOfWork.VerificationCodes.UpdateAsync(userVerification);   
        
        var sendUserVerification = await _unitOfWork.VerificationCodes.GetAsync(new VerificationCodeFilter 
            { UserId = command.UserId });
        
        var user  = await _unitOfWork.Users.GetAsync(new UserFilter 
            { Id = command.UserId });
        
        await _emailService.SendEmailAsync(
            user.Email,
            "Your verification code",
            $"You have reset your email, please verify again.<br>" +
                 $"Your verification code is <strong>{sendUserVerification.VerificationToken}</strong>"
        );
    }
}