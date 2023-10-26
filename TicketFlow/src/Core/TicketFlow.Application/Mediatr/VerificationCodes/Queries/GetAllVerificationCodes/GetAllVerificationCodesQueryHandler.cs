using AutoMapper;
using MediatR;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Queries.GetAllVerificationCodes;

public class GetAllVerificationCodesQueryHandler : IRequestHandler<GetAllVerificationCodesQuery, GetAllVerificationCodesVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllVerificationCodesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<GetAllVerificationCodesVm> Handle(GetAllVerificationCodesQuery query, 
        CancellationToken cancellationToken = default)
    {
        var verificationCodes = await _unitOfWork.VerificationCodes
            .GetAllAsync(new VerificationCodeFilter(), cancellationToken) ?? new List<VerificationCode>();
        
        return new GetAllVerificationCodesVm 
            { VerificationCodes = _mapper.Map<IList<VerificationCodeVm>>(verificationCodes) };
    }
}