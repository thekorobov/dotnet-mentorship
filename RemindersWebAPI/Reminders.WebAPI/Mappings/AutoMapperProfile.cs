using AutoMapper;
using Reminders.BLL.CQS.Reminders.Commands.CreateReminder;
using Reminders.BLL.CQS.Reminders.Commands.UpdateReminder;
using Reminders.BLL.CQS.Users.Commands.CreateUser;
using Reminders.BLL.CQS.Users.Commands.ResetUserEmail;
using Reminders.BLL.CQS.Users.Commands.UpdateUser;
using Reminders.BLL.CQS.VerificationCodes.Commands.CreateVerificationCode;
using Reminders.BLL.CQS.VerificationCodes.Commands.ResetVerificationCode;
using Reminders.BLL.CQS.VerificationCodes.Commands.VerifyVerificationCode;
using Reminders.BLL.DTO;
using Reminders.DAL.Entities;
using Reminders.WebAPI.Models;

namespace Reminders.WebAPI.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Reminder, ReminderDto>();
        CreateMap<ReminderDto, Reminder>();
        CreateMap<ReminderDto, ReminderModel>();
        CreateMap<ReminderModel, ReminderDto>();
        
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<UserDto, UserModel>();
        CreateMap<UserModel, UserDto>();
        CreateMap<User, UserModel>();
        CreateMap<UserModel, User>();
        
        CreateMap<VerificationCode, VerificationCodeDto>();
        CreateMap<VerificationCodeDto, VerificationCode>();
        
        CreateMap<CreateReminderCommand, Reminder>();
        CreateMap<UpdateReminderCommand, Reminder>();
        
        CreateMap<CreateUserCommand, User>();
        CreateMap<ResetUserPasswordCommand, User>();
        CreateMap<ResetUserEmailCommand, User>();
        
        CreateMap<VerifyVerificationCodeCommand, VerificationCode>();
        CreateMap<CreateVerificationCodeCommand, VerificationCode>();
        CreateMap<ResetVerificationCodeCommand, VerificationCode>();
    }
}