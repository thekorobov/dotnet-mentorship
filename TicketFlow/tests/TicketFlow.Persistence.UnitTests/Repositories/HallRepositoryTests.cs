namespace TicketFlow.Persistence.UnitTests.Repositories;

public class HallRepositoryTests : IClassFixture<RepositoriesFixture>
{
    private readonly RepositoriesFixture _fixture;
    public HallRepositoryTests(RepositoriesFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task CreateAsync_ValidHall_HallCreated()
    {
        // Arrange
        var hall = new Hall
        {
            Id = HallConstants.Hall5Id,
            VenueId = VenueConstants.Venue9Id,
            Name = HallConstants.Hall5Name,
            SeatingCapacity = HallConstants.Hall5SeatingCapacity
        };

        // Act
        var hallId = await _fixture.UnitOfWork.Halls.CreateAsync(hall);

        // Assert
        Assert.NotNull(hallId);
        Assert.Equal(hall.Id, hallId);
    }
    
    [Fact]
    public async Task CreateAsync_NullHall_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.UnitOfWork.Halls.CreateAsync(null));
        Assert.Equal("hall", exception.ParamName);
    }
    
    [Fact]
    public async Task UpdateAsync_ValidHall_HallUpdated()
    {
        // Arrange
        var newHallName = "newHallName";
        var hall = new Hall
        {
            Id = HallConstants.Hall6Id,
            VenueId = VenueConstants.Venue9Id,
            Name = HallConstants.Hall6Name,
            SeatingCapacity = HallConstants.Hall6SeatingCapacity
        };
        await _fixture.UnitOfWork.Halls.CreateAsync(hall);
        hall.Name = newHallName;

        // Act
        await _fixture.UnitOfWork.Halls.UpdateAsync(hall);

        // Assert
        var updatedHall = await _fixture.Context.Halls.FindAsync(hall.Id);
        Assert.Equal(newHallName, updatedHall!.Name);
    }
    
    [Fact]
    public async Task UpdateAsync_NullHall_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>
            (() => _fixture.UnitOfWork.Halls.UpdateAsync(null));
        Assert.Equal("hall", exception.ParamName);
    }

    [Fact]
    public async Task UpdateAsync_HallNotFound_EntityNotFoundException()
    {
        // Arrange
        var nonExistingHallId = HallConstants.NonExistingHallId;
        var hall = new Hall { Id = nonExistingHallId };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _fixture.UnitOfWork.Halls.UpdateAsync(hall));
        Assert.Equal($"Entity <Hall> ({nonExistingHallId}) not found.", exception.Message);
    }
    
    [Fact]
    public async Task GetAsync_ById_HallExists_HallReturned()
    {
        // Arrange
        var hall = new Hall
        {
            Id = HallConstants.Hall7Id,
            VenueId = VenueConstants.Venue9Id,
            Name = HallConstants.Hall7Name,
            SeatingCapacity = HallConstants.Hall7SeatingCapacity
        };
        await _fixture.UnitOfWork.Halls.CreateAsync(hall);

        // Act
        var foundHall = await _fixture.UnitOfWork.Halls.GetAsync(new HallFilter { Id = hall.Id });

        // Assert
        Assert.NotNull(foundHall);
        Assert.Equal(hall.Id, foundHall.Id);
    }
    
    [Fact]
    public async Task GetAsync_ById_IncludeSeats_HallsAndSeatsReturned()
    {
        // Arrange
        var hallId = HallConstants.Hall8Id;
        var seat1 = new Seat
        {
            Id = SeatConstants.Seat1Id, 
            HallId = hallId,
            Row = SeatConstants.Seat1Row,
            Number = SeatConstants.Seat1Number,
            Status = SeatConstants.Seat1Status
        };
        var seat2 = new Seat
        {
            Id = SeatConstants.Seat2Id,
            HallId = hallId,
            Row = SeatConstants.Seat2Row,
            Number = SeatConstants.Seat2Number,
            Status = SeatConstants.Seat2Status
        };
        
        var hall = new Hall
        {
            Id = hallId, 
            VenueId = VenueConstants.Venue9Id,
            Name = HallConstants.Hall8Name,
            SeatingCapacity = HallConstants.Hall8SeatingCapacity,
            Seats = new List<Seat> { seat1, seat2 }
        };
        await _fixture.UnitOfWork.Halls.CreateAsync(hall);

        // Act
        var foundHall = await _fixture.UnitOfWork.Halls.GetAsync(new HallFilter { Id = hall.Id, IncludeSeats = true });

        // Assert
        Assert.NotNull(foundHall);
        Assert.Equal(hall.Id, foundHall.Id);
        Assert.NotNull(foundHall.Seats);
        Assert.Equal(2, foundHall.Seats.Count);
    }
    
    [Fact]
    public async Task GetAsync_ById_IncludeVenue_HallsAndVenueReturned()
    {
        // Arrange
        var venueId = VenueConstants.Venue9Id;
        var hall = new Hall
        {
            Id = HallConstants.Hall9Id, 
            VenueId = venueId,
            Name = HallConstants.Hall9Name,
            SeatingCapacity = HallConstants.Hall9SeatingCapacity
        };
        await _fixture.UnitOfWork.Halls.CreateAsync(hall);

        // Act
        var foundHall = await _fixture.UnitOfWork.Halls.GetAsync(new HallFilter { Id = hall.Id, IncludeVenue = true });

        // Assert
        Assert.NotNull(foundHall);
        Assert.Equal(hall.Id, foundHall.Id);
        Assert.NotNull(foundHall.Venue);
        Assert.Equal(venueId, foundHall.Venue.Id);
    }
    
    [Fact]
    public async Task GetAsync_InvalidId_NullReturned()
    {
        // Act
        var foundHall = await _fixture.UnitOfWork.Halls.GetAsync(new HallFilter { Id = HallConstants.NonExistingHallId });

        // Assert
        Assert.Null(foundHall);
    }
    
    [Fact]
    public async Task GetAllAsync_ById_HallsExist_VenuesReturned()
    {
        // Arrange
        var venueId = VenueConstants.Venue10Id;
        var hall1 = new Hall
        {
            Id = HallConstants.Hall10Id, 
            VenueId = venueId,
            Name = HallConstants.Hall10Name,
            SeatingCapacity = HallConstants.Hall10SeatingCapacity
        };
        var hall2 = new Hall
        {
            Id = HallConstants.Hall11Id, 
            VenueId = venueId,
            Name = HallConstants.Hall11Name,
            SeatingCapacity = HallConstants.Hall11SeatingCapacity
        };
        await _fixture.UnitOfWork.Halls.CreateAsync(hall1);
        await _fixture.UnitOfWork.Halls.CreateAsync(hall2);
        
        // Act
        var allHalls = await _fixture.UnitOfWork.Halls.GetAllAsync(new HallFilter { VenueId = venueId });

        // Assert
        Assert.NotNull(allHalls);
        Assert.Equal(2, allHalls.Count());
    }
    
    [Fact]
    public async Task GetAllAsync_IncludeSeats_HallsExist_HallsAndSeatsReturned()
    {
        // Arrange
        var venueId = VenueConstants.Venue11Id;
        var hallId1 = HallConstants.Hall12Id;
        var hallId2 = HallConstants.Hall13Id;
        
        var seat1 = new Seat
        {
            Id = SeatConstants.Seat3Id, 
            HallId = hallId1,
            Row = SeatConstants.Seat3Row,
            Number = SeatConstants.Seat3Number,
            Status = SeatConstants.Seat3Status
        };
        var seat2 = new Seat
        {
            Id = SeatConstants.Seat4Id,
            HallId = hallId2,
            Row = SeatConstants.Seat4Row,
            Number = SeatConstants.Seat4Number,
            Status = SeatConstants.Seat4Status
        };
        
        var hall1 = new Hall
        {
            Id = hallId1, 
            VenueId = venueId,
            Name = HallConstants.Hall12Name,
            SeatingCapacity = HallConstants.Hall12SeatingCapacity,
            Seats = new List<Seat> { seat1 }
        };
        var hall2 = new Hall
        {
            Id = hallId2, 
            VenueId = venueId,
            Name = HallConstants.Hall13Name,
            SeatingCapacity = HallConstants.Hall13SeatingCapacity,
            Seats = new List<Seat> { seat2 }
        };
        await _fixture.UnitOfWork.Halls.CreateAsync(hall1);
        await _fixture.UnitOfWork.Halls.CreateAsync(hall2);
        
        // Act
        var allHalls = await _fixture.UnitOfWork.Halls.GetAllAsync(
            new HallFilter { VenueId = venueId, IncludeSeats = true});

        // Assert
        Assert.NotNull(allHalls);
        Assert.Equal(2, allHalls.Count());
        foreach (var hall in allHalls)
        {
            Assert.NotNull(hall.Seats);
            Assert.Equal(1, hall.Seats.Count);   
        }
    }
    
    [Fact]
    public async Task DeleteAsync_HallExists_HallDeleted()
    {
        // Arrange
        var hallId = HallConstants.Hall14Id;
        var hall = new Hall
        {
            Id = hallId, 
            VenueId = VenueConstants.Venue11Id,
            Name = HallConstants.Hall14Name,
            SeatingCapacity = HallConstants.Hall14SeatingCapacity
        };
        await _fixture.UnitOfWork.Halls.CreateAsync(hall);
        
        // Act
        await _fixture.UnitOfWork.Halls.DeleteAsync(hallId);

        // Assert
        var deletedHall = await _fixture.Context.Halls.FindAsync(hallId);
        Assert.Null(deletedHall);
    }
    
    [Fact]
    public async Task DeleteAsync_NonExistingHall_EntityNotFoundException()
    {
        // Arrange
        var nonExistingHallId = HallConstants.NonExistingHallId;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _fixture.UnitOfWork.Halls.DeleteAsync(nonExistingHallId)
        );
        Assert.Equal($"Entity <Hall> ({nonExistingHallId}) not found.", exception.Message);
    }
    
    [Fact]
    public async Task DeleteAsync_NullHallId_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _fixture.UnitOfWork.Halls.DeleteAsync(null)
        );
        Assert.Equal("id", exception.ParamName);
    }
    
    [Fact]
    public void GetQueryable_ShouldReturnQueryable()
    {
        // Arrange
        // Act
        var queryable = _fixture.UnitOfWork.Halls.GetQueryable();

        // Assert
        Assert.NotNull(queryable);
    }
}