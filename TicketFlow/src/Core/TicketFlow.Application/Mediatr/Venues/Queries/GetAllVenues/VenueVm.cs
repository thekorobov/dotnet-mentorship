using AutoMapper;
using TicketFlow.Application.Common.Mappings;
using TicketFlow.Application.Mediatr.Halls.Queries.GetAllHall;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Application.Mediatr.Venues.Queries.GetAllVenues;

public class VenueVm : IMapWith<Venue>
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public int SeatingCapacity { get; set; }
    public ICollection<HallVm> Halls { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Venue, VenueVm>()
            .ForMember(query => query.Id,
                opt => opt.MapFrom(venue => venue.Id))
            .ForMember(query => query.UserId,
                opt => opt.MapFrom(venue => venue.UserId))
            .ForMember(query => query.Name,
                opt => opt.MapFrom(venue => venue.Name))
            .ForMember(query => query.Address,
                opt => opt.MapFrom(venue => venue.Address))
            .ForMember(query => query.SeatingCapacity,
                opt => opt.MapFrom(venue => venue.SeatingCapacity))
            .ForMember(query => query.Halls,
                opt => opt.MapFrom(venue => venue.Halls));
    }
}