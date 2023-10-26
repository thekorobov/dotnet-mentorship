using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Queries.GetVerificationCode;

public class GetVerificationCodeQueryHandler : IRequestHandler<GetVerificationCodeQuery, GetVerificationCodeVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVerificationCodeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetVerificationCodeVm> Handle(GetVerificationCodeQuery query, CancellationToken cancellationToken = default)
    {
        var verificationCodeFilter = new VerificationCodeFilter
        {
            UserId = query.UserId,
            Id = query.VerificationCodeId,
            VerificationToken = query.VerificationToken
        };

        var verificationCode = await _unitOfWork.VerificationCodes.GetAsync(verificationCodeFilter, cancellationToken);
    
        if (verificationCode == null)
        {
            var identifier = !string.IsNullOrEmpty(query.VerificationCodeId) ? 
                query.VerificationCodeId : query.VerificationToken;
            throw new EntityNotFoundException(nameof(VerificationCode), identifier);
        }

        return _mapper.Map<GetVerificationCodeVm>(verificationCode);
    }
}