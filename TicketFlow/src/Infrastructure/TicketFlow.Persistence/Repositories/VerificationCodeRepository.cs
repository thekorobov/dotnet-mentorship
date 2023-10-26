using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Common.Interfaces;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Persistence.Repositories;

public class VerificationCodeRepository : IVerificationCodeRepository
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public VerificationCodeRepository(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<string> CreateAsync(VerificationCode verificationCode, CancellationToken cancellationToken = default)
    {
        if (verificationCode == null)
        {
            throw new ArgumentNullException(nameof(verificationCode));
        }

        await _context.VerificationCodes.AddAsync(verificationCode, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return verificationCode.Id;
    }

    public async Task UpdateAsync(VerificationCode verificationCode, CancellationToken cancellationToken = default)
    {
        if (verificationCode == null)
        {
            throw new ArgumentNullException(nameof(verificationCode));
        }
        
        var updatedVerificationCode = await _context.VerificationCodes.FindAsync(verificationCode!.Id);

        if (updatedVerificationCode == null)
        {
            throw new EntityNotFoundException(nameof(VerificationCode), verificationCode.Id);
        }

        _mapper.Map(verificationCode, updatedVerificationCode);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<VerificationCode> GetAsync(VerificationCodeFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.VerificationCodes.AsQueryable();
        
        if (!string.IsNullOrEmpty(filter.Id))
        {
            query = query.Where(vc => vc.Id == filter.Id);
        }
        if (!string.IsNullOrEmpty(filter.VerificationToken))
        {
            query = query.Where(vc => vc.VerificationToken == filter.VerificationToken);
        }
        if (!string.IsNullOrEmpty(filter.UserId))
        {
            query = query.Where(vc => vc.UserId == filter.UserId);
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<VerificationCode>> GetAllAsync(
        VerificationCodeFilter filter, CancellationToken cancellationToken = default)
    {
        return await _context.VerificationCodes.ToListAsync(cancellationToken);
    }

    public IQueryable<VerificationCode> GetQueryable()
    {
        throw new NotImplementedException();
    }
}