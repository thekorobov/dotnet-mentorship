using Reminders.BLL.CQS.Users.Commands.CreateUser;

namespace Reminders.UnitTests.CQS.Users.Commands.CreateUser;

public class CreateUserHandlerTests
{
    private readonly Mock<IUserStore<User>> _userStoreMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IRoleStore<IdentityRole<int>>> _roleStoreMock;
    private readonly Mock<RoleManager<IdentityRole<int>>> _roleManagerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateUserHandler _createUserHandler;
    
    public CreateUserHandlerTests()
    {
        _userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock =
            new Mock<UserManager<User>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
        _roleStoreMock = new Mock<IRoleStore<IdentityRole<int>>>();
        _roleManagerMock = new Mock<RoleManager<IdentityRole<int>>>(_roleStoreMock.Object, null, null, null, null);
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _createUserHandler = new CreateUserHandler(_unitOfWorkMock.Object, _mapperMock.Object, _userManagerMock.Object,
            _roleManagerMock.Object);
    }
    
    [Theory]
    [InlineData(AuthProviderType.SimpleAuth, Constants.ValidPassword)]
    [InlineData(AuthProviderType.GoogleAuth, null)]
    public async Task HandleAsync_UserNotExists_SuccessResultReturned(AuthProviderType authProvider, string password)
    {
        // Arrange
        var command = CreateCommand(authProvider, password);
        SetupMocksForSuccessCase(authProvider);

        // Act
        await _createUserHandler.HandleAsync(command);

        // Assert
        _userManagerMock.Verify(um => um.AddToRolesAsync(It.IsAny<User>(), 
            It.IsAny<IEnumerable<string>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Users.GetAsync(It.IsAny<UserFilter>()), Times.Once);
    }

    [Theory]
    [InlineData(AuthProviderType.SimpleAuth)]
    [InlineData(AuthProviderType.GoogleAuth)]
    public async Task HandleAsync_UserAlreadyExists_FailureResultReturned(AuthProviderType authProvider)
    {
        // Arrange
        var command = CreateCommand(authProvider, Constants.ValidPassword);
        _unitOfWorkMock.Setup(u => u.Users.GetAsync(It.IsAny<UserFilter>())).ReturnsAsync(new User());

        // Act & Assert
        await Assert.ThrowsAsync<UserAlreadyExistsException>(() => _createUserHandler.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_InvalidData_ValidationExceptionThrown()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Users.GetAsync(It.IsAny<UserFilter>())).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(async () => 
            await _createUserHandler.HandleAsync(new CreateUserCommand
        {
            Email = "",
            UserName = "",
            Password = "",
            AuthProviderType = AuthProviderType.SimpleAuth
        }));
    }
    
    private CreateUserCommand CreateCommand(AuthProviderType authProvider, string password)
    {
        return new CreateUserCommand
        {
            Email = Constants.ValidEmail,
            UserName = Constants.ValidUserName,
            Password = password,
            AuthProviderType = authProvider
        };
    }
    
    private void SetupMocksForSuccessCase(AuthProviderType authProviderType)
    {
        _unitOfWorkMock.Setup(u => u.Users.GetAsync(It.IsAny<UserFilter>()))
            .ReturnsAsync((User)null);
        _mapperMock.Setup(m => m.Map<CreateUserCommand, User>(It.IsAny<CreateUserCommand>()))
            .Returns(new User { });
        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), 
            It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        SetupRoleMocks();
        
        if (authProviderType == AuthProviderType.SimpleAuth)
        {
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), 
                It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        }
        else
        {
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);
        }

        _userManagerMock.Setup(um => um.AddToRolesAsync(It.IsAny<User>(), 
            It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success);
    }
    
    private void SetupRoleMocks()
    {
        var roles = new List<IdentityRole<int>>
        {
            new IdentityRole<int> { Id = 1, Name = "Admin" },
            new IdentityRole<int> { Id = 2, Name = "User" }
        };
        
        var rolesDbSetMock = new Mock<DbSet<IdentityRole<int>>>();
        rolesDbSetMock.As<IQueryable<IdentityRole<int>>>().Setup(m => m.Provider)
            .Returns(roles.AsQueryable().Provider);
        rolesDbSetMock.As<IQueryable<IdentityRole<int>>>().Setup(m => m.Expression)
            .Returns(roles.AsQueryable().Expression);
        rolesDbSetMock.As<IQueryable<IdentityRole<int>>>().Setup(m => m.ElementType)
            .Returns(roles.AsQueryable().ElementType);
        rolesDbSetMock.As<IQueryable<IdentityRole<int>>>().Setup(m => m.GetEnumerator())
            .Returns(roles.AsQueryable().GetEnumerator());
        
        _roleManagerMock.Setup(rm => rm.Roles).Returns(rolesDbSetMock.Object);
    }
}