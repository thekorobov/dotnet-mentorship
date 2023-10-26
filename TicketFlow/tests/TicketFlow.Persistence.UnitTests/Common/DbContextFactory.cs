namespace TicketFlow.Persistence.UnitTests.Common;

public abstract class DbContextFactory
{
    public static ApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        
        context.Database.EnsureCreated();

        context.Users.AddRangeAsync(
            new User
            {
                Id = UserConstants.User6Id,
                Role = UserConstants.User6Role,
                Email = UserConstants.User6Email,
                Forename = UserConstants.User6Forename,
                Surname = UserConstants.User6Surname,
                UserName = UserConstants.User6UserName,
                PasswordHash = UserConstants.User6PasswordHash,
                AuthProviderType = UserConstants.User6AuthProviderType
            },
            new User
            {
                Id = UserConstants.User7Id,
                Role = UserConstants.User7Role,
                Email = UserConstants.User7Email,
                Forename = UserConstants.User7Forename,
                Surname = UserConstants.User7Surname,
                UserName = UserConstants.User7UserName,
                PasswordHash = UserConstants.User7PasswordHash,
                AuthProviderType = UserConstants.User7AuthProviderType
            },
            new User
            {
                Id = UserConstants.User8Id,
                Role = UserConstants.User8Role,
                Email = UserConstants.User8Email,
                Forename = UserConstants.User8Forename,
                Surname = UserConstants.User8Surname,
                UserName = UserConstants.User8UserName,
                PasswordHash = UserConstants.User8PasswordHash,
                AuthProviderType = UserConstants.User8AuthProviderType
            },
            new User
            {
                Id = UserConstants.User9Id,
                Role = UserConstants.User9Role,
                Email = UserConstants.User9Email,
                Forename = UserConstants.User9Forename,
                Surname = UserConstants.User9Surname,
                UserName = UserConstants.User9UserName,
                PasswordHash = UserConstants.User9PasswordHash,
                AuthProviderType = UserConstants.User9AuthProviderType
            }
        );

        context.Venues.AddRangeAsync(
            new Venue
            {
                Id = VenueConstants.Venue9Id,
                UserId = UserConstants.User9Id,
                Name = VenueConstants.Venue9Name,
                Address = VenueConstants.Venue9Address,
                SeatingCapacity = VenueConstants.Venue9SeatingCapacity
            },
            new Venue
            {
                Id = VenueConstants.Venue10Id,
                UserId = UserConstants.User8Id,
                Name = VenueConstants.Venue10Name,
                Address = VenueConstants.Venue10Address,
                SeatingCapacity = VenueConstants.Venue10SeatingCapacity
            },
            new Venue
            {
                Id = VenueConstants.Venue11Id,
                UserId = UserConstants.User7Id,
                Name = VenueConstants.Venue11Name,
                Address = VenueConstants.Venue11Address,
                SeatingCapacity = VenueConstants.Venue11SeatingCapacity
            }
        );

        context.Halls.AddRangeAsync
        (
            new Hall
            {
                Id = HallConstants.Hall15Id, 
                VenueId = VenueConstants.Venue9Id,
                Name = HallConstants.Hall15Name,
                SeatingCapacity = HallConstants.Hall15SeatingCapacity
            },
            new Hall
            {
                Id = HallConstants.Hall16Id, 
                VenueId = VenueConstants.Venue9Id,
                Name = HallConstants.Hall16Name,
                SeatingCapacity = HallConstants.Hall16SeatingCapacity
            },
            new Hall
            {
                Id = HallConstants.Hall17Id, 
                VenueId = VenueConstants.Venue9Id,
                Name = HallConstants.Hall17Name,
                SeatingCapacity = HallConstants.Hall17SeatingCapacity
            },
            new Hall
            {
                Id = HallConstants.Hall18Id, 
                VenueId = VenueConstants.Venue9Id,
                Name = HallConstants.Hall18Name,
                SeatingCapacity = HallConstants.Hall18SeatingCapacity
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