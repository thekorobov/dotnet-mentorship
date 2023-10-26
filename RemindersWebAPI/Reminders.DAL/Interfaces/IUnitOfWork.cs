namespace Reminders.DAL.Interfaces;

public interface IUnitOfWork
{
    IReminderRepository Reminders { get; }
    IUserRepository Users { get; }
    IVerificationCodeRepository VerificationCodes { get; }
    Task SaveChangesAsync();
}