using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TicketFlow.Application.Common.Mappings;

namespace TicketFlow.Application.Mediatr.Halls.Commands.CreateHall;

public record CreateHallDto : IMapWith<CreateHallCommand>
{
    [Required]
    public string VenueId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public int SeatingCapacity { get; set; }
    [Required]
    public int RowsCount { get; set; }
    [Required]
    public int SeatsPerRow { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateHallDto, CreateHallCommand>()
            .ForMember(dest => dest.VenueId, 
                opt => opt.MapFrom(src => src.VenueId))
            .ForMember(dest => dest.Name, 
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.SeatingCapacity, 
                opt => opt.MapFrom(src => src.SeatingCapacity))
            .ForMember(dest => dest.RowsCount, 
                opt => opt.MapFrom(src => src.RowsCount))
            .ForMember(dest => dest.SeatsPerRow, 
                opt => opt.MapFrom(src => src.SeatsPerRow));
    }
}