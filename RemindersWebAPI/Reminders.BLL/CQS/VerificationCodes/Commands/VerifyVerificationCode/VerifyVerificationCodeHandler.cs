using AutoMapper;
using Reminders.BLL.DTO.Exceptions;
using Reminders.BLL.Interfaces;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.VerificationCodes.Commands.VerifyVerificationCode;

public class VerifyVerificationCodeHandler : ICommandHandler<VerifyVerificationCodeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VerifyVerificationCodeHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task HandleAsync(VerifyVerificationCodeCommand command)
    {
        var existingUserVerification  = await _unitOfWork.VerificationCodes.GetAsync(new VerificationCodeFilter 
            { VerificationToken = command.VerificationToken });
        
        if (existingUserVerification  == null)
        {
            throw new EntityNotFoundException(nameof(VerificationCode));
        }
        
        if (existingUserVerification .VerifiedAt.HasValue)
        {
            throw new UserAlreadyVerifiedException("You have already been verified.");
        }
        
        var userVerify = _mapper.Map<VerifyVerificationCodeCommand, VerificationCode>(command);
        if (userVerify != null) 
        {
            userVerify.UserId = existingUserVerification.UserId;
            userVerify.VerifiedAt = DateTime.UtcNow;
            userVerify.VerificationToken = String.Empty;
        }
        
        await _unitOfWork.VerificationCodes.UpdateAsync(userVerify);
    }
}