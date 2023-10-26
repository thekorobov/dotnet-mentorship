namespace Reminders.UnitTests.Common;

public class ReminderHandlerFixture : IDisposable
{
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Mock<IMapper> MockMapper { get; }
    public Mock<IReminderRepository> MockReminderRepository { get; }

    public ReminderHandlerFixture()
    {
        MockUnitOfWork = new Mock<IUnitOfWork>();
        MockMapper = new Mock<IMapper>();
        MockReminderRepository = new Mock<IReminderRepository>();

        MockUnitOfWork.Setup(uow => uow.Reminders).Returns(MockReminderRepository.Object);
    }

    public void Dispose() {}
}