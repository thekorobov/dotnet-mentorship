using AutoMapper;
using TicketFlow.Application.Common.Mappings;

namespace TicketFlow.Application.Mediatr.Halls.Commands.UpdateHall;

public record UpdateHallDto : IMapWith<UpdateHallCommand>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int SeatingCapacity { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateHallDto, UpdateHallCommand>()
            .ForMember(dest => dest.Id, 
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, 
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.SeatingCapacity, 
                opt => opt.MapFrom(src => src.SeatingCapacity));
    }
}