using AutoMapper;
using TicketFlow.Application.Common.Mappings;
using TicketFlow.Domain.Enums;

namespace TicketFlow.Application.Mediatr.Seat.Commands.UpdateSeat;

public class UpdateSeatDto : IMapWith<UpdateSeatCommand>
{
    public string Id { get; set; }
    public string HallId { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public SeatStatus Status { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateSeatDto, UpdateSeatCommand>()
            .ForMember(dest => dest.Id, 
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.HallId, 
                opt => opt.MapFrom(src => src.HallId))
            .ForMember(dest => dest.Row, 
                opt => opt.MapFrom(src => src.Row))
            .ForMember(dest => dest.Number, 
                opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.Status, 
                opt => opt.MapFrom(src => src.Status));
    }
}