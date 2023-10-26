using AutoMapper;
using TicketFlow.Application.Mediatr.Halls.Commands.CreateHall;
using TicketFlow.Application.Mediatr.Halls.Commands.UpdateHall;
using TicketFlow.Application.Mediatr.Seat.Commands.CreateSeat;
using TicketFlow.Application.Mediatr.Seat.Commands.UpdateSeat;
using TicketFlow.Application.Mediatr.Users.Commands.CreateUser;
using TicketFlow.Application.Mediatr.Venues.Commands.CreateVenue;
using TicketFlow.Application.Mediatr.Venues.Commands.UpdateVenue;
using TicketFlow.Application.Mediatr.VerificationCodes.Commands.CreateVerificationCode;
using TicketFlow.Application.Mediatr.VerificationCodes.Commands.ResetVerificationCode;
using TicketFlow.Application.Mediatr.VerificationCodes.Commands.VerifyVerificationCode;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Application.Common.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        MapUserModels();
        MapVerificationCodeModels();
        MapVenueModels();
        MapHallModels();
        MapSeatModels();
    }
    
    private void MapUserModels()
    {
        CreateMap<CreateUserCommand, User>();
    }
    
    private void MapVerificationCodeModels()
    {
        CreateMap<VerificationCode, VerificationCode>();
        CreateMap<CreateVerificationCodeCommand, VerificationCode>();
        CreateMap<VerifyVerificationCodeCommand, VerificationCode>();
        CreateMap<ResetVerificationCodeCommand, VerificationCode>();
    }
    
    private void MapVenueModels()
    {
        CreateMap<Venue, Venue>();
        CreateMap<CreateVenueCommand, Venue>();
        CreateMap<UpdateVenueCommand, Venue>();
    }
    
    private void MapHallModels()
    {
        CreateMap<Hall, Hall>();
        CreateMap<CreateHallCommand, Hall>();
        CreateMap<UpdateHallCommand, Hall>();
    }
    
    private void MapSeatModels()
    {
        CreateMap<Seat, Seat>();
        CreateMap<CreateSeatCommand, Seat>();
        CreateMap<UpdateSeatCommand, Seat>();
    }
}