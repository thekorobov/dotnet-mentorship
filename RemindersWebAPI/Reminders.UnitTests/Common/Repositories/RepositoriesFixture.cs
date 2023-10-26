namespace Reminders.UnitTests.Common;

public class RepositoriesFixture : IDisposable
{
    public ApplicationDbContext Context { get; }
    public ReminderRepository ReminderRepository { get; }
    public UserRepository UserRepository { get; }
    public VerificationCodeRepository VerificationCodeRepository { get; }

    public RepositoriesFixture()
    {
        Context = DbContextFactory.Create();
        ReminderRepository = new ReminderRepository(Context);
        UserRepository = new UserRepository(Context);
        VerificationCodeRepository = new VerificationCodeRepository(Context);
    }

    public void Dispose()
    {
        DbContextFactory.Destroy(Context);
    }
}