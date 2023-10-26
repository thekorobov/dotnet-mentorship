using AutoMapper;
using TicketFlow.Application.Common.Mappings;

namespace TicketFlow.Application.Mediatr.Venues.Commands.UpdateVenue;

public record UpdateVenueDto : IMapWith<UpdateVenueCommand>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int SeatingCapacity { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateVenueDto, UpdateVenueCommand>()
            .ForMember(dest => dest.Id, 
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, 
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Address, 
                opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.SeatingCapacity, 
                opt => opt.MapFrom(src => src.SeatingCapacity));
    }
}