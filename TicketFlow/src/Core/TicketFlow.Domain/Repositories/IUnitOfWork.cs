namespace TicketFlow.Domain.Repositories;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IVerificationCodeRepository VerificationCodes { get; }
    IVenueRepository Venues { get; }
    IHallRepository Halls { get; }
    ISeatRepository Seats { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken);
}