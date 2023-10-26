namespace TicketFlow.Persistence.UnitTests.Common;

public class RepositoriesFixture : IDisposable
{
    public IMapper Mapper { get; }
    public UnitOfWork UnitOfWork { get; }
    public ApplicationDbContext Context;

    public RepositoriesFixture()
    {
        Context = DbContextFactory.Create();
        Mapper = Substitute.For<IMapper>();
        UnitOfWork = new UnitOfWork(Context, Mapper);
    }

    public void Dispose()
    {
        DbContextFactory.Destroy(Context);
    }
}