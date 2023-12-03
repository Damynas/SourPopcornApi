using Application.Users.Abstractions;
using Domain.Auth.Constants;
using Domain.Users.DataTransferObjects.Requests;
using FluentValidation;
using Presentation.Users.DataTransferObjects;

namespace Presentation.Users.Validators;

public class CreateUserRequestBodyValidator : AbstractValidator<CreateUserRequestBody>
{
    private readonly IUserService _userService;

    private readonly List<string> _allowedRoles = [Role.User, Role.Moderator, Role.Admin];

    public CreateUserRequestBodyValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(20).WithMessage("Username cannot exceed 20 characters.")
            .MustAsync(UsernameIsUniqueAsync).WithMessage("Username is already in use.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[@#$%^&+=]+").WithMessage("Your password must contain at least one special character.");
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required.")
            .MaximumLength(20).WithMessage("Display name cannot exceed 20 characters.");
        RuleFor(x => x.Roles)
            .Must(RolesAreValid)
            .WithMessage($"Roles are not valid and must be an array containing at least one of these roles: {string.Join("", _allowedRoles.Select((role, index) => index + 1 == _allowedRoles.Count ? $"{role}" : $"{role}, "))}.")
            .Must(UserRoleIsPresent)
            .WithMessage("Role 'User' must be one of the roles.");

    }

    private async Task<bool> UsernameIsUniqueAsync(string username, CancellationToken cancellationToken)
    {
        var request = new GetUserByUsernameRequest(username);
        var result = await _userService.GetUserByUsernameAsync(request, cancellationToken);
        return result.IsFailure;
    }

    private bool RolesAreValid(List<string>? roles)
    {
        var isValid = true;
        roles?.ForEach(role =>
        {
            if(!_allowedRoles.Contains(role))
            {
                isValid = false;
            }
        });
        return isValid;
    }

    private bool UserRoleIsPresent(List<string>? roles)
    {
        return roles is null || roles.Contains(Role.User);
    }
}
