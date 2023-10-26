using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TicketFlow.Application.Common.Mappings;
using TicketFlow.Domain.Enums;

namespace TicketFlow.Application.Mediatr.Seat.Commands.CreateSeat;

public record CreateSeatDto : IMapWith<CreateSeatCommand>
{
    [Required]
    public string HallId { get; set; }
    [Required]
    public int Row { get; set; }
    [Required]
    public int Number { get; set; }
    [Required]
    public SeatStatus Status { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateSeatDto, CreateSeatCommand>()
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