using Microsoft.EntityFrameworkCore;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Event> Events { get; set; }
    DbSet<Venue> Venues { get; set; }
    DbSet<Hall> Halls { get; set; }
    DbSet<Seat> Seats { get; set; }
    DbSet<Notification> Notifications { get; set; }
    DbSet<Payment> Payments { get; set; }
    DbSet<Ticket> Tickets { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<VerificationCode> VerificationCodes { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken); 
}