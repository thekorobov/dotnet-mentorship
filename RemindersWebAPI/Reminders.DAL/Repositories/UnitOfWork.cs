using Reminders.DAL.Data;
using Reminders.DAL.Interfaces;

namespace Reminders.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private ReminderRepository _reminderRepository;
    private UserRepository _userRepository;
    private VerificationCodeRepository _verificationCodeRepository;
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReminderRepository Reminders
    {
        get
        {
            if (_reminderRepository == null)
            {
                _reminderRepository = new ReminderRepository(_context);
            }
            return _reminderRepository;
        }
    }
    
    public IUserRepository Users
    {
        get
        {
            if (_userRepository == null)
            {
                _userRepository = new UserRepository(_context);
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
                _verificationCodeRepository = new VerificationCodeRepository(_context);
            }
            return _verificationCodeRepository;
        }
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}