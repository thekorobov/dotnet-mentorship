using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Common.Exceptions.VerificationCodes;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Commands.VerifyVerificationCode;

public class VerifyVerificationCodeCommandHandler : IRequestHandler<VerifyVerificationCodeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VerifyVerificationCodeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(VerifyVerificationCodeCommand command, CancellationToken cancellationToken = default)
    {
        var existingUserVerification  = await _unitOfWork.VerificationCodes.GetAsync(new VerificationCodeFilter 
            { VerificationToken = command.VerificationToken }, cancellationToken);
        
        if (existingUserVerification == null)
        {
            throw new EntityNotFoundException(nameof(VerificationCode), command.VerificationToken);
        }
        
        if (existingUserVerification.VerifiedAt.HasValue)
        {
            throw new UserAlreadyVerifiedException("You have already been verified.");
        }
        
        var userVerify = _mapper.Map<VerifyVerificationCodeCommand, VerificationCode>(command);
        if (userVerify != null)
        {
            userVerify.UserId = existingUserVerification.UserId;
            userVerify.Id = existingUserVerification.Id;
            userVerify.VerifiedAt = DateTime.UtcNow;
            userVerify.VerificationToken = String.Empty;
        }
        
        await _unitOfWork.VerificationCodes.UpdateAsync(userVerify!, cancellationToken);
        
        return Unit.Value; 
    }
}