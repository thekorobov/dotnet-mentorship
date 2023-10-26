using AutoMapper;
using TicketFlow.Application.Common.Mappings;
using TicketFlow.Domain.Enums;

namespace TicketFlow.Application.Mediatr.Seat.Queries.GetSeat;

public record GetSeatVm : IMapWith<Domain.Entities.Seat>
{
    public string Id { get; set; }
    public string HallId { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public SeatStatus Status { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Seat, GetSeatVm>()
            .ForMember(query => query.Id,
                opt => opt.MapFrom(venue => venue.Id))
            .ForMember(query => query.HallId,
                opt => opt.MapFrom(venue => venue.HallId))
            .ForMember(query => query.Row,
                opt => opt.MapFrom(venue => venue.Row))
            .ForMember(query => query.Number,
                opt => opt.MapFrom(venue => venue.Number))
            .ForMember(query => query.Status,
                opt => opt.MapFrom(venue => venue.Status));
    }
}