using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;

namespace TicketFlow.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User, UserFilter> { }