namespace TicketFlow.Persistence.UnitTests.Repositories;

public class SeatRepositoryTests : IClassFixture<RepositoriesFixture>
{
    private readonly RepositoriesFixture _fixture;
    public SeatRepositoryTests(RepositoriesFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task CreateAsync_ValidSeat_SeatCreated()
    {
        // Arrange
        var seat = new Seat
        {
            Id = SeatConstants.Seat5Id,
            HallId = HallConstants.Hall15Id,
            Row = SeatConstants.Seat5Row,
            Number = SeatConstants.Seat5Number,
            Status = SeatConstants.Seat5Status
        };

        // Act
        var seatId = await _fixture.UnitOfWork.Seats.CreateAsync(seat);

        // Assert
        Assert.NotNull(seatId);
        Assert.Equal(seat.Id, seatId);
    }
    
    [Fact]
    public async Task CreateAsync_NullHall_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.UnitOfWork.Seats.CreateAsync(null));
        Assert.Equal("seat", exception.ParamName);
    }
    
    [Fact]
    public async Task CreateRangeAsync_ValidSeats_SeatsCreated()
    {
        // Arrange
        var hallId = HallConstants.Hall16Id;
        var seats = new List<Seat>()
        {
            new Seat
            {
                Id = SeatConstants.Seat6Id,
                HallId = hallId,
                Row = SeatConstants.Seat6Row,
                Number = SeatConstants.Seat6Number,
                Status = SeatConstants.Seat6Status
            },
            new Seat
            {
                Id = SeatConstants.Seat7Id,
                HallId = hallId,
                Row = SeatConstants.Seat7Row,
                Number = SeatConstants.Seat7Number,
                Status = SeatConstants.Seat7Status
            }
        };

        // Act
        await _fixture.UnitOfWork.Seats.CreateRangeAsync(seats);

        // Assert
        var foundSeats = await _fixture.UnitOfWork.Halls.GetAsync(new HallFilter { Id = hallId, IncludeSeats = true });
        
        Assert.NotNull(foundSeats);
        Assert.Equal(hallId, foundSeats.Id);
        Assert.NotNull(foundSeats.Seats);
        Assert.Equal(2, foundSeats.Seats.Count);
    }
    
    [Fact]
    public async Task CreateRangeAsync_NullHall_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.UnitOfWork.Seats.CreateRangeAsync(null));
        Assert.Equal("seats", exception.ParamName);
    }
    
    [Fact]
    public async Task UpdateAsync_ValidSeat_SeatUpdated()
    {
        // Arrange
        var newSeatStatus = SeatStatus.Occupied;
        var seat = new Seat
        {
            Id = SeatConstants.Seat8Id,
            HallId = HallConstants.Hall17Id,
            Row = SeatConstants.Seat8Row,
            Number = SeatConstants.Seat8Number,
            Status = SeatConstants.Seat8Status
        };
        await _fixture.UnitOfWork.Seats.CreateAsync(seat);
        seat.Status = newSeatStatus;

        // Act
        await _fixture.UnitOfWork.Seats.UpdateAsync(seat);

        // Assert
        var updatedHall = await _fixture.Context.Seats.FindAsync(seat.Id);
        Assert.Equal(newSeatStatus, updatedHall!.Status);
    }
    
    [Fact]
    public async Task UpdateAsync_NullSeat_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>
            (() => _fixture.UnitOfWork.Seats.UpdateAsync(null));
        Assert.Equal("seat", exception.ParamName);
    }

    [Fact]
    public async Task UpdateAsync_SeatNotFound_EntityNotFoundException()
    {
        // Arrange
        var nonExistingSeatId = SeatConstants.NonExistingSeatId;
        var seat = new Seat { Id = nonExistingSeatId };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _fixture.UnitOfWork.Seats.UpdateAsync(seat));
        Assert.Equal($"Entity <Seat> ({nonExistingSeatId}) not found.", exception.Message);
    }
    
    [Fact]
    public async Task GetAsync_ById_SeatExists_SeatReturned()
    {
        // Arrange
        var seat = new Seat
        {
            Id = SeatConstants.Seat9Id,
            HallId = HallConstants.Hall17Id,
            Row = SeatConstants.Seat9Row,
            Number = SeatConstants.Seat9Number,
            Status = SeatConstants.Seat9Status
        };
        await _fixture.UnitOfWork.Seats.CreateAsync(seat);

        // Act
        var foundSeat = await _fixture.UnitOfWork.Seats.GetAsync(new SeatFilter { Id = seat.Id });

        // Assert
        Assert.NotNull(foundSeat);
        Assert.Equal(seat.Id, foundSeat.Id);
    }
    
    [Fact]
    public async Task GetAsync_ById_IncludeHall_HallAndSeatReturned()
    {
        // Arrange
        var hallId = HallConstants.Hall17Id;
        var seat = new Seat
        {
            Id = SeatConstants.Seat10Id,
            HallId = hallId,
            Row = SeatConstants.Seat10Row,
            Number = SeatConstants.Seat10Number,
            Status = SeatConstants.Seat10Status
        };
        await _fixture.UnitOfWork.Seats.CreateAsync(seat);

        // Act
        var foundSeat = await _fixture.UnitOfWork.Seats.GetAsync(new SeatFilter { Id = seat.Id, IncludeHall = true });

        // Assert
        Assert.NotNull(foundSeat);
        Assert.Equal(seat.Id, foundSeat.Id);
        Assert.NotNull(foundSeat.Hall);
        Assert.Equal(hallId, foundSeat.Hall.Id);
    }
    
    [Fact]
    public async Task GetAsync_InvalidId_NullReturned()
    {
        // Act
        var foundSeat = await _fixture.UnitOfWork.Seats.GetAsync(new SeatFilter { Id = SeatConstants.NonExistingSeatId });

        // Assert
        Assert.Null(foundSeat);
    }
    
    [Fact]
    public async Task GetAllAsync_ById_SeatsExist_SeatsReturned()
    {
        // Arrange
        var hallId = HallConstants.Hall18Id;
        var seat1 = new Seat
        {
            Id = SeatConstants.Seat11Id,
            HallId = hallId,
            Row = SeatConstants.Seat11Row,
            Number = SeatConstants.Seat11Number,
            Status = SeatConstants.Seat11Status
        };
        var seat2 = new Seat
        {
            Id = SeatConstants.Seat12Id,
            HallId = hallId,
            Row = SeatConstants.Seat12Row,
            Number = SeatConstants.Seat12Number,
            Status = SeatConstants.Seat12Status
        };
        await _fixture.UnitOfWork.Seats.CreateAsync(seat1);
        await _fixture.UnitOfWork.Seats.CreateAsync(seat2);
        
        // Act
        var allSeats = await _fixture.UnitOfWork.Seats.GetAllAsync(new SeatFilter { HallId = hallId });

        // Assert
        Assert.NotNull(allSeats);
        Assert.Equal(2, allSeats.Count());
    }
    
    [Fact]
    public async Task DeleteAsync_SeatExists_SeatDeleted()
    {
        // Arrange
        var seatId = SeatConstants.Seat13Id;
        var seat = new Seat
        {
            Id = seatId,
            HallId = HallConstants.Hall18Id,
            Row = SeatConstants.Seat13Row,
            Number = SeatConstants.Seat13Number,
            Status = SeatConstants.Seat13Status
        };
        await _fixture.UnitOfWork.Seats.CreateAsync(seat);
        
        // Act
        await _fixture.UnitOfWork.Seats.DeleteAsync(seatId);

        // Assert
        var deletedSeat = await _fixture.Context.Seats.FindAsync(seatId);
        Assert.Null(deletedSeat);
    }
    
    [Fact]
    public async Task DeleteAsync_NonExistingSeat_EntityNotFoundException()
    {
        // Arrange
        var nonExistingSeatId = SeatConstants.NonExistingSeatId;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _fixture.UnitOfWork.Seats.DeleteAsync(nonExistingSeatId)
        );
        Assert.Equal($"Entity <Seat> ({nonExistingSeatId}) not found.", exception.Message);
    }
    
    [Fact]
    public async Task DeleteAsync_NullSeatId_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _fixture.UnitOfWork.Seats.DeleteAsync(null)
        );
        Assert.Equal("id", exception.ParamName);
    }
    
    [Fact]
    public void GetQueryable_ShouldReturnQueryable()
    {
        // Arrange
        // Act
        var queryable = _fixture.UnitOfWork.Seats.GetQueryable();

        // Assert
        Assert.NotNull(queryable);
    }
}