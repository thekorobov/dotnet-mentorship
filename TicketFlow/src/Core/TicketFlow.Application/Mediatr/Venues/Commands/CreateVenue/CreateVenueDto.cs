using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TicketFlow.Application.Common.Mappings;

namespace TicketFlow.Application.Mediatr.Venues.Commands.CreateVenue;

public record CreateVenueDto : IMapWith<CreateVenueCommand>
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public int SeatingCapacity { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateVenueDto, CreateVenueCommand>()
            .ForMember(dest => dest.Name, 
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Address, 
                opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.SeatingCapacity, 
                opt => opt.MapFrom(src => src.SeatingCapacity));
    }
}