using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Reminders.BLL.DTO.Exceptions;
using Reminders.BLL.Interfaces;
using Reminders.BLL.Utils;
using Reminders.BLL.Utils.Constants;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.Users.Commands.CreateUser;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public CreateUserHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task<int> HandleAsync(CreateUserCommand command)
    {
        if (command.AuthProviderType != AuthProviderType.GoogleAuth)
        {
            await ValidationHelper.ValidateCommandAsync(command, new CreateUserValidator());
        }
        
        var filter = new UserFilter { Email = command.Email };
        var existingUser = await _unitOfWork.Users.GetAsync(filter);

        if (existingUser != null)
        {
            throw new UserAlreadyExistsException("User with this email already exists.");
        }
        
        var user = _mapper.Map<CreateUserCommand, User>(command);
        user.UserName = UsernameGenerateHelper.GenerateUsername(command.UserName);
        user.Role = UserRoles.User; 
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
            
        var addToRolesResult = await _userManager.AddToRolesAsync(user, userRoles);

        if (!addToRolesResult.Succeeded)
        {
            throw new RoleAssignmentFailedException("Failed to add user to roles: " + string.Join(", ", addToRolesResult.Errors.Select(x => x.Description)));
        }

        return user.Id;
    }
}