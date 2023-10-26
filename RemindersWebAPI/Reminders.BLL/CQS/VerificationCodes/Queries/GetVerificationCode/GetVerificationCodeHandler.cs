using AutoMapper;
using Reminders.BLL.DTO;
using Reminders.BLL.Interfaces;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.VerificationCodes.Queries.GetVerificationCode;

public class GetVerificationCodeHandler : IQueryHandler<GetVerificationCodeQuery, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVerificationCodeHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<string> HandleAsync(GetVerificationCodeQuery query)
    {
        if (string.IsNullOrEmpty(query.UserId.ToString()) && string.IsNullOrEmpty(query.VerificationToken))
        {
            throw new ArgumentException("Id or email must be provided.");
        }
        
        VerificationCode verificationCode = null;
        
        if (query.UserId.HasValue)
        {
            verificationCode = await _unitOfWork.VerificationCodes.GetAsync(new VerificationCodeFilter() { UserId = query.UserId });
        }
        else if (!string.IsNullOrEmpty(query.VerificationToken))
        {
            verificationCode = await _unitOfWork.VerificationCodes.GetAsync(new VerificationCodeFilter { VerificationToken = query.VerificationToken });
        }
        
        return verificationCode.VerificationToken;
    }
}