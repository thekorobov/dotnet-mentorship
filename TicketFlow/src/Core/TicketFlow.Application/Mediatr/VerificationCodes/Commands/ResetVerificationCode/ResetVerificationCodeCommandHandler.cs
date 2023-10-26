using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Utils.Generators;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;
using TicketFlow.Infrastructure.Shared.Services.Email.Abstractions;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Commands.ResetVerificationCode;

public class ResetVerificationCodeCommandHandler : IRequestHandler<ResetVerificationCodeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public ResetVerificationCodeCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<Unit> Handle(ResetVerificationCodeCommand command, CancellationToken cancellationToken = default)
    {
        if (command.UserId != command.CurrentUserId)
        {
            throw new PermissionDeniedException("You don't have permission to update this user verification code.");
        }
        
        var existingUserVerification  = await _unitOfWork.VerificationCodes
            .GetAsync(new VerificationCodeFilter { UserId = command.UserId }, cancellationToken);
        
        if (existingUserVerification == null)
        {
            throw new EntityNotFoundException(nameof(VerificationCode));
        }
        
        existingUserVerification.VerifiedAt = null;
        existingUserVerification.VerificationToken = VerificationCodeGenerator.GenerateVerificationCode();
            
        await _unitOfWork.VerificationCodes.UpdateAsync(existingUserVerification, cancellationToken);   
        
        var user = await _unitOfWork.Users.GetAsync(new UserFilter { Id = command.UserId }, cancellationToken);
        
        await _emailService.SendEmailAsync(
            user.Email!,
            "TicketFlow Verification Code Reset",
            $"Hello {user.Surname} {user.Forename},<br><br>" +
            $"It seems like you've reset your email. No worries! <br>" +
            $"Please verify your email again by entering the following verification code in the app:<br><br>" +
            $"<strong>{existingUserVerification.VerificationToken}</strong><br><br>" +
            "Thank you for your patience! 🙏<br>" +
            "<em>The TicketFlow Team</em>"
        );
        
        return Unit.Value;
    }
}