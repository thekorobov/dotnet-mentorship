using AutoMapper;
using TicketFlow.Application.Common.Mappings;
using TicketFlow.Domain.Enums;

namespace TicketFlow.Application.Mediatr.Seat.Queries.GetAllSeats;

public record SeatVm : IMapWith<Domain.Entities.Seat>
{
    public string Id { get; set; }
    public string HallId { get; set; }
    public int Number { get; set; }
    public int Row { get; set; }
    public SeatStatus Status { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Seat, SeatVm>()
            .ForMember(query => query.Id,
                opt => opt.MapFrom(hall => hall.Id))
            .ForMember(query => query.HallId,
                opt => opt.MapFrom(hall => hall.HallId))
            .ForMember(query => query.Row,
                opt => opt.MapFrom(hall => hall.Row))
            .ForMember(query => query.Number,
                opt => opt.MapFrom(hall => hall.Number))
            .ForMember(query => query.Status,
                opt => opt.MapFrom(hall => hall.Status));
    }
}