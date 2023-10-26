using AutoMapper;
using TicketFlow.Application.Common.Mappings;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Application.Mediatr.Halls.Queries.GetAllHall;

public record HallVm : IMapWith<Hall>
{
    public string Id { get; set; }
    public string VenueId { get; set; }
    public string? Name { get; set; }
    public int SeatingCapacity { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Hall, HallVm>()
            .ForMember(query => query.Id,
                opt => opt.MapFrom(hall => hall.Id))
            .ForMember(query => query.VenueId,
                opt => opt.MapFrom(hall => hall.VenueId))
            .ForMember(query => query.Name,
                opt => opt.MapFrom(hall => hall.Name))
            .ForMember(query => query.SeatingCapacity,
                opt => opt.MapFrom(hall => hall.SeatingCapacity));
    }
}