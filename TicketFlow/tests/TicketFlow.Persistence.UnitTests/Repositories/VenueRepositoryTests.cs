namespace TicketFlow.Persistence.UnitTests.Repositories;

public class VenueRepositoryTests : IClassFixture<RepositoriesFixture>
{
    private readonly RepositoriesFixture _fixture;
    public VenueRepositoryTests(RepositoriesFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task CreateAsync_ValidVenue_VenueCreated()
    {
        // Arrange
        var venue = new Venue
        {
            Id = VenueConstants.Venue1Id,
            UserId = UserConstants.User1Id,
            Name = VenueConstants.Venue1Name,
            Address = VenueConstants.Venue1Address,
            SeatingCapacity = VenueConstants.Venue1SeatingCapacity
        };

        // Act
        var venueId = await _fixture.UnitOfWork.Venues.CreateAsync(venue);

        // Assert
        Assert.NotNull(venueId);
        Assert.Equal(venue.Id, venueId);
    }
    
    [Fact]
    public async Task CreateAsync_NullVenue_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.UnitOfWork.Venues.CreateAsync(null));
        Assert.Equal("venue", exception.ParamName);
    }
    
    [Fact]
    public async Task UpdateAsync_ValidVenue_VenueUpdated()
    {
        // Arrange
        var newVenueName = "newVenueName";
        var venue = new Venue
        {
            Id = VenueConstants.Venue2Id,
            UserId = UserConstants.User2Id,
            Name = VenueConstants.Venue2Name,
            Address = VenueConstants.Venue2Address,
            SeatingCapacity = VenueConstants.Venue2SeatingCapacity
        };
        await _fixture.UnitOfWork.Venues.CreateAsync(venue);
        venue.Name = newVenueName;

        // Act
        await _fixture.UnitOfWork.Venues.UpdateAsync(venue);

        // Assert
        var updatedVenue = await _fixture.Context.Venues.FindAsync(venue.Id);
        Assert.Equal(newVenueName, updatedVenue!.Name);
    }
    
    [Fact]
    public async Task UpdateAsync_NullVenue_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>
            (() => _fixture.UnitOfWork.Venues.UpdateAsync(null));
        Assert.Equal("venue", exception.ParamName);
    }

    [Fact]
    public async Task UpdateAsync_VenueNotFound_EntityNotFoundException()
    {
        // Arrange
        var nonExistingVenueId1 = VenueConstants.NonExistingVenueId;
        var venue = new Venue { Id = nonExistingVenueId1 };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _fixture.UnitOfWork.Venues.UpdateAsync(venue));
        Assert.Equal($"Entity <Venue> ({nonExistingVenueId1}) not found.", exception.Message);
    }
    
    [Fact]
    public async Task GetAsync_ById_VenueExists_VenueReturned()
    {
        // Arrange
        var venue = new Venue
        {
            Id = VenueConstants.Venue3Id,
            UserId = UserConstants.User3Id,
            Name = VenueConstants.Venue3Name,
            Address = VenueConstants.Venue3Address,
            SeatingCapacity = VenueConstants.Venue3SeatingCapacity
        };
        await _fixture.UnitOfWork.Venues.CreateAsync(venue);

        // Act
        var foundVenue = await _fixture.UnitOfWork.Venues.GetAsync(new VenueFilter { Id = venue.Id });

        // Assert
        Assert.NotNull(foundVenue);
        Assert.Equal(venue.Id, foundVenue.Id);
    }
    
    [Fact]
    public async Task GetAsync_ById_IncludeHalls_VenueAndHallsReturned()
    {
        // Arrange
        var hall1 = new Hall
        {
            Id = HallConstants.Hall1Id, 
            VenueId = VenueConstants.Venue4Id,
            Name = HallConstants.Hall1Name,
            SeatingCapacity = HallConstants.Hall1SeatingCapacity
        };
        var hall2 = new Hall
        {
            Id = HallConstants.Hall2Id, 
            VenueId = VenueConstants.Venue4Id,
            Name = HallConstants.Hall2Name,
            SeatingCapacity = HallConstants.Hall2SeatingCapacity
        };
        
        var venue = new Venue
        {
            Id = VenueConstants.Venue4Id,
            UserId = UserConstants.User4Id,
            Name = VenueConstants.Venue4Name,
            Address = VenueConstants.Venue4Address,
            SeatingCapacity = VenueConstants.Venue4SeatingCapacity,
            Halls = new List<Hall> { hall1, hall2 }
        };
        await _fixture.UnitOfWork.Venues.CreateAsync(venue);

        // Act
        var foundVenue = await _fixture.UnitOfWork.Venues.GetAsync(new VenueFilter { Id = venue.Id, IncludeHalls = true });

        // Assert
        Assert.NotNull(foundVenue);
        Assert.Equal(venue.Id, foundVenue.Id);
        Assert.NotNull(foundVenue.Halls);
        Assert.Equal(2, foundVenue.Halls.Count);  
        Assert.Null(foundVenue.Halls.First().Seats);
    }
    
    [Fact]
    public async Task GetAsync_InvalidId_NullReturned()
    {
        // Act
        var foundVenue = await _fixture.UnitOfWork.Venues.GetAsync(new VenueFilter { Id = VenueConstants.NonExistingVenueId });

        // Assert
        Assert.Null(foundVenue);
    }
    
    [Fact]
    public async Task GetAllAsync_ById_VenuesExist_VenuesReturned()
    {
        // Arrange
        var userId = UserConstants.User5Id;
        var venue1 = new Venue
        {
            Id = VenueConstants.Venue5Id,
            UserId = userId,
            Name = VenueConstants.Venue5Name,
            Address = VenueConstants.Venue5Address,
            SeatingCapacity = VenueConstants.Venue5SeatingCapacity
        };
        var venue2 = new Venue
        {
            Id = VenueConstants.Venue6Id,
            UserId = userId,
            Name = VenueConstants.Venue6Name,
            Address = VenueConstants.Venue6Address,
            SeatingCapacity = VenueConstants.Venue6SeatingCapacity
        };
        await _fixture.UnitOfWork.Venues.CreateAsync(venue1);
        await _fixture.UnitOfWork.Venues.CreateAsync(venue2);
        
        // Act
        var allVenues = await _fixture.UnitOfWork.Venues.GetAllAsync(new VenueFilter { UserId = userId });

        // Assert
        Assert.NotNull(allVenues);
        Assert.Equal(2, allVenues.Count());
    }
    
    [Fact]
    public async Task GetAllAsync_IncludeHalls_VenuesExist_VenuesReturned()
    {
        // Arrange
        var userId = UserConstants.User6Id;
        
        var hall1 = new Hall
        {
            Id = HallConstants.Hall3Id, 
            VenueId = VenueConstants.Venue7Id,
            Name = HallConstants.Hall3Name,
            SeatingCapacity = HallConstants.Hall3SeatingCapacity
        };
        var hall2 = new Hall
        {
            Id = HallConstants.Hall4Id, 
            VenueId = VenueConstants.Venue8Id,
            Name = HallConstants.Hall4Name,
            SeatingCapacity = HallConstants.Hall4SeatingCapacity
        };
        
        var venue1 = new Venue
        {
            Id = VenueConstants.Venue7Id,
            UserId = userId,
            Name = VenueConstants.Venue7Name,
            Address = VenueConstants.Venue7Address,
            SeatingCapacity = VenueConstants.Venue7SeatingCapacity,
            Halls = new List<Hall> { hall1 }
        };
        var venue2 = new Venue
        {
            Id = VenueConstants.Venue8Id,
            UserId = userId,
            Name = VenueConstants.Venue8Name,
            Address = VenueConstants.Venue8Address,
            SeatingCapacity = VenueConstants.Venue8SeatingCapacity,
            Halls = new List<Hall> { hall2 }
        };
        await _fixture.UnitOfWork.Venues.CreateAsync(venue1);
        await _fixture.UnitOfWork.Venues.CreateAsync(venue2);
        
        // Act
        var allVenues = await _fixture.UnitOfWork.Venues.GetAllAsync(
            new VenueFilter { UserId = userId, IncludeHalls = true});

        // Assert
        Assert.NotNull(allVenues);
        Assert.Equal(2, allVenues.Count());
        foreach (var venue in allVenues)
        {
            Assert.NotNull(venue.Halls);
            Assert.Equal(1, venue.Halls.Count);  
            Assert.Null(venue.Halls.First().Seats);  
        }
    }
    
    [Fact]
    public async Task DeleteAsync_VenueExists_VenueDeleted()
    {
        // Arrange
        var userId = UserConstants.User7Id;
        var venueId = UserConstants.User7Id;
        var venue = new Venue
        {
            Id = venueId,
            UserId = userId,
            Name = VenueConstants.Venue7Name,
            Address = VenueConstants.Venue7Address,
            SeatingCapacity = VenueConstants.Venue7SeatingCapacity
        };
        await _fixture.UnitOfWork.Venues.CreateAsync(venue);
        
        // Act
        await _fixture.UnitOfWork.Venues.DeleteAsync(venueId);

        // Assert
        var deletedVenue = await _fixture.Context.Venues.FindAsync(venueId);
        Assert.Null(deletedVenue);
    }
    
    [Fact]
    public async Task DeleteAsync_NonExistingVenue_EntityNotFoundException()
    {
        // Arrange
        var nonExistingVenueId1 = VenueConstants.NonExistingVenueId;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _fixture.UnitOfWork.Venues.DeleteAsync(nonExistingVenueId1)
        );
        Assert.Equal($"Entity <Venue> ({nonExistingVenueId1}) not found.", exception.Message);
    }
    
    [Fact]
    public async Task DeleteAsync_NullVenueId_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _fixture.UnitOfWork.Venues.DeleteAsync(null)
        );
        Assert.Equal("id", exception.ParamName);
    }
    
    [Fact]
    public void GetQueryable_ShouldReturnQueryable()
    {
        // Arrange
        // Act
        var queryable = _fixture.UnitOfWork.Venues.GetQueryable();

        // Assert
        Assert.NotNull(queryable);
    }
}