namespace Reminders.UnitTests.Common;

public class UserHandlerFixture : IDisposable
{
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Mock<UserManager<User>> MockUserManager { get; }
    public Mock<IMapper> MockMapper { get; }
    public Mock<IUserRepository> MockUserRepository { get; }
    public Mock<IVerificationCodeRepository> MockUserVerificationRepository { get; }
    
    public UserHandlerFixture()
    {
        MockUnitOfWork = new Mock<IUnitOfWork>();
        
        var userStoreMock = new Mock<IUserStore<User>>();
        MockUserManager = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        
        MockMapper = new Mock<IMapper>();

        MockUserRepository = new Mock<IUserRepository>();
        MockUnitOfWork.Setup(uow => uow.Users).Returns(MockUserRepository.Object);

        MockUserVerificationRepository = new Mock<IVerificationCodeRepository>();
        MockUnitOfWork.Setup(uow => uow.VerificationCodes).Returns(MockUserVerificationRepository.Object);
    }

    public void Dispose() {}
}