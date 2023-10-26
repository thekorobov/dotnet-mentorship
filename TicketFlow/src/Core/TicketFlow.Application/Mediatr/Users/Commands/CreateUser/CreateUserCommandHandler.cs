using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TicketFlow.Application.Common.Exceptions.Users;
using TicketFlow.Application.Mediatr.VerificationCodes.Commands.CreateVerificationCode;
using TicketFlow.Application.Utils.Generators;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<string>> _roleManager;
    private readonly IMediator _mediator;
    
    public CreateUserCommandHandler(IUnitOfWork unitOfWork, 
        IMapper mapper, 
        UserManager<User> userManager, 
        RoleManager<IdentityRole<string>> roleManager, 
        IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
        _mediator = mediator;
    }
    
    public async Task<string> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        var existingUser = await _unitOfWork.Users.GetAsync(new UserFilter { Email = command.Email }, cancellationToken);

        if (existingUser != null)
        {
            throw new UserAlreadyExistsException("User with this email already exists.");
        }
        
        var user = _mapper.Map<CreateUserCommand, User>(command);
        user.UserName = UsernameGenerator.GenerateUsername(command.UserName);
        user.AuthProviderType = command.AuthProviderType;
        
        IdentityResult createResult;
         
        if (command.AuthProviderType == AuthProviderType.SimpleAuth)
        {
            createResult = await _userManager.CreateAsync(user, command.Password);
        }
        else
        {
            createResult = await _userManager.CreateAsync(user);
        }
            
        if (!createResult.Succeeded)
        {
            throw new UserCreationFailedException("Failed to create user: " + string.Join(", ", createResult.Errors.Select(x => x.Description)));
        }   
        
        var userRoles = _roleManager.Roles
            .Where(role => role.Name == user.Role)
            .Select(role => role.Name)
            .ToList();
            
        var addToRolesResult = await _userManager.AddToRolesAsync(user, userRoles!);

        if (!addToRolesResult.Succeeded)
        {
            throw new RoleAssignmentFailedException("Failed to add user to roles: " + string.Join(", ", addToRolesResult.Errors.Select(x => x.Description)));
        }

        await _mediator.Send(new CreateVerificationCodeCommand { UserId = user.Id }, cancellationToken);

        return user.Id;
    }
}