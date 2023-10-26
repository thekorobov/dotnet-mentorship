using Microsoft.EntityFrameworkCore;
using Reminders.DAL.Data;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.DAL.Repositories;

public class VerificationCodeRepository : IVerificationCodeRepository
{
    private readonly ApplicationDbContext _context;

    public VerificationCodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> CreateAsync(VerificationCode? entity)
    {
        if (entity == null)
        {
            return 0;
        }
        
        await _context.VerificationCodes.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity?.Id ?? 0; 
    }

    public async Task UpdateAsync(VerificationCode entity)
    {
        var updatedVerification = await _context.VerificationCodes.FirstOrDefaultAsync(uv => uv.UserId == entity!.UserId);
        if (updatedVerification != null)
        {
            updatedVerification.VerificationToken = entity.VerificationToken;
            updatedVerification.VerifiedAt = entity.VerifiedAt;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<VerificationCode> GetAsync(VerificationCodeFilter codeFilter)
    {
        var query = _context.VerificationCodes.AsQueryable();

        if (codeFilter.UserId.HasValue)
        {
            query = query.Where(u => u.UserId == codeFilter.UserId.Value);
        }
        
        if (!string.IsNullOrEmpty(codeFilter.VerificationToken))
        {
            query = query.Where(u => u.VerificationToken == codeFilter.VerificationToken);
        }

        return await query.FirstOrDefaultAsync();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
    
    public IQueryable<VerificationCode> GetQueryable()
    {
        return _context.VerificationCodes.AsQueryable();
    }
    
    public async Task<IEnumerable<VerificationCode>> GetAllAsync()
    {
        return await _context.VerificationCodes.ToListAsync();
    }
}