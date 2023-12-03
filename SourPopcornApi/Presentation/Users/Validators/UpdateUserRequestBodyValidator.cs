using Domain.Auth.Constants;
using FluentValidation;
using Presentation.Users.DataTransferObjects;

namespace Presentation.Users.Validators;

public class UpdateUserRequestBodyValidator : AbstractValidator<UpdateUserRequestBody>
{
    private readonly List<string> _allowedRoles = [Role.User, Role.Moderator, Role.Admin];

    public UpdateUserRequestBodyValidator()
    {
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required.")
            .MaximumLength(20).WithMessage("Display name cannot exceed 20 characters.");
        RuleFor(x => x.Roles)
            .Must(RolesAreValid)
            .WithMessage($"Roles are not valid and must be an array containing at least one of these roles: {string.Join("", _allowedRoles.Select((role, index) => index + 1 == _allowedRoles.Count ? $"{role}" : $"{role}, "))}.")
            .Must(UserRoleIsPresent)
            .WithMessage("Role 'User' must be one of the roles.");
    }

    private bool RolesAreValid(List<string>? roles)
    {
        var isValid = true;
        roles?.ForEach(role =>
        {
            if (!_allowedRoles.Contains(role))
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
