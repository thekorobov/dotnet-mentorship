using AutoMapper;
using TicketFlow.Application.Common.Mappings;
using TicketFlow.Application.Mediatr.Seat.Queries.GetAllSeats;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Application.Mediatr.Halls.Queries.GetHall;

public record GetHallVm : IMapWith<Hall>
{
    public string Id { get; set; }
    public string VenueId { get; set; }
    public string? Name { get; set; }
    public int SeatingCapacity { get; set; }
    public ICollection<SeatVm> Seats { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Hall, GetHallVm>()
            .ForMember(query => query.Id,
                opt => opt.MapFrom(venue => venue.Id))
            .ForMember(query => query.VenueId,
                opt => opt.MapFrom(venue => venue.VenueId))
            .ForMember(query => query.Name,
                opt => opt.MapFrom(venue => venue.Name))
            .ForMember(query => query.SeatingCapacity,
                opt => opt.MapFrom(venue => venue.SeatingCapacity))
            .ForMember(query => query.Seats,
                opt => opt.MapFrom(venue => venue.Seats));
    }
}