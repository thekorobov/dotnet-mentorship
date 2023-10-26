using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Common.Exceptions.VerificationCodes;
using TicketFlow.Application.Utils.Generators;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;
using TicketFlow.Infrastructure.Shared.Services.Email.Abstractions;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Commands.CreateVerificationCode;

public class CreateVerificationCodeCommandHandler : IRequestHandler<CreateVerificationCodeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    
    public CreateVerificationCodeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<Unit> Handle(CreateVerificationCodeCommand command, CancellationToken cancellationToken = default)
    {
        var existingUser = await _unitOfWork.Users.GetAsync(new UserFilter { Id = command.UserId }, cancellationToken);
        if (existingUser == null)
        {
            throw new EntityNotFoundException(nameof(User), command.UserId);
        }

        var existingUserVerification = await _unitOfWork.VerificationCodes
            .GetAsync(new VerificationCodeFilter { UserId = command.UserId }, cancellationToken);
        
        if (existingUserVerification != null && existingUserVerification.VerifiedAt.HasValue)
        { 
            throw new UserAlreadyVerifiedException("User is already verified.");
        }
        
        var userVerification = _mapper.Map<CreateVerificationCodeCommand, VerificationCode>(command);
        userVerification.VerificationToken = VerificationCodeGenerator.GenerateVerificationCode();
        
        await _unitOfWork.VerificationCodes.CreateAsync(userVerification, cancellationToken);
        
        if (existingUser.AuthProviderType == AuthProviderType.SimpleAuth)
        {
            await _emailService.SendEmailAsync(
                existingUser.Email!,
                "TicketFlow Verification Code",
                $"Hello {existingUser.Surname} {existingUser.Forename},<br><br>" +
                $"Thank you for choosing TicketFlow! To proceed, please verify your email by entering the following verification code in the app:<br><br>" +
                $"<strong>{userVerification.VerificationToken}</strong><br><br>" +
                "Happy ticketing! 🎉<br>" +
                "<em>The TicketFlow Team</em>"
            );
        }
        
        return Unit.Value; 
    }
}