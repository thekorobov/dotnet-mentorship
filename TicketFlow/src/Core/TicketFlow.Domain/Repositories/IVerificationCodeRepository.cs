using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;

namespace TicketFlow.Domain.Repositories;

public interface IVerificationCodeRepository : IBaseRepository<VerificationCode, VerificationCodeFilter>
{
    
}