namespace Reminders.UnitTests.Common;

public class DbContextFactory
{
    public static int UserIdUserA = 1;
    public static int UserIdUserB = 2;

    public static ApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        
        var hasher = new PasswordHasher<User>();
        
        context.Database.EnsureCreated();
        context.Users.AddRangeAsync(
            new User
            {
                Id = UserIdUserA,
                Role = "User",
                Email = "user-a@gmail.com",
                UserName = "user-a",
                PasswordHash = hasher.HashPassword(null, "Test12345!"),
                AuthProviderType = AuthProviderType.SimpleAuth
            },
            new User
            {
                Id = UserIdUserB,
                Role = "User",
                Email = "user-b@gmail.com",
                UserName = "user-b",
                PasswordHash = "",
                AuthProviderType = AuthProviderType.GoogleAuth
            }
        );

        context.Reminders.AddRangeAsync(
            new Reminder
            {
                Id = 1,
                UserId = UserIdUserA,
                Name = Constants.FirstValidReminderName,
                Date = DateTime.Now.AddMinutes(10)
            },
            new Reminder
            {
                Id = 2,
                UserId = UserIdUserB,
                Name = Constants.SecondValidReminderName,
                Date = DateTime.Now.AddMinutes(20)
            }
        );
        
        context.SaveChangesAsync();
        return context;
    }
    
    public static void Destroy(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }

}