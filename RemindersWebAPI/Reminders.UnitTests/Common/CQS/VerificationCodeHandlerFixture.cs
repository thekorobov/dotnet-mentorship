namespace Reminders.UnitTests.Common;

public class VerificationCodeHandlerFixture : IDisposable
{
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Mock<IMapper> MockMapper { get; }
    public Mock<IUserRepository> MockUserRepository { get; }
    public Mock<IVerificationCodeRepository> MockUserVerificationRepository { get; }
    public Mock<IEmailService> MockEmailService { get; }
    
    public VerificationCodeHandlerFixture()
    {
        MockUnitOfWork = new Mock<IUnitOfWork>();
        MockMapper = new Mock<IMapper>();
        
        MockUserRepository = new Mock<IUserRepository>();
        MockUnitOfWork.Setup(uow => uow.Users).Returns(MockUserRepository.Object);

        MockUserVerificationRepository = new Mock<IVerificationCodeRepository>();
        MockUnitOfWork.Setup(uow => uow.VerificationCodes).Returns(MockUserVerificationRepository.Object);

        MockEmailService = new Mock<IEmailService>();
    }

    public void Dispose() {}
}