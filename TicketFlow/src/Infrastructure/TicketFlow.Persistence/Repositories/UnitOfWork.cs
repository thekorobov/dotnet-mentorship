using AutoMapper;
using TicketFlow.Application.Common.Interfaces;
using TicketFlow.Domain.Repositories;
using TicketFlow.Persistence.Data;

namespace TicketFlow.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private UserRepository _userRepository;
    private VerificationCodeRepository _verificationCodeRepository;
    private VenueRepository _venueRepository;
    private HallRepository _hallRepository;
    private SeatRepository _seatRepository;
 
    public UnitOfWork(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
 
    public IUserRepository Users
    {
        get
        {
            if (_userRepository == null)
            {
                _userRepository = new UserRepository(_context, _mapper);
            }
            return _userRepository;
        }
    }

    public IVerificationCodeRepository VerificationCodes
    {
        get
        {
            if (_verificationCodeRepository == null)
            {
                _verificationCodeRepository = new VerificationCodeRepository(_context, _mapper);
            }
            return _verificationCodeRepository;
        }
    }
    
    public IVenueRepository Venues
    {
        get
        {
            if (_venueRepository == null)
            {
                _venueRepository = new VenueRepository(_context);
            }
            return _venueRepository;
        }
    }
    
    public IHallRepository Halls
    {
        get
        {
            if (_hallRepository == null)
            {
                _hallRepository = new HallRepository(_context);
            }
            return _hallRepository;
        }
    }
    
    public ISeatRepository Seats
    {
        get
        {
            if (_seatRepository == null)
            {
                _seatRepository = new SeatRepository(_context);
            }
            return _seatRepository;
        }
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}