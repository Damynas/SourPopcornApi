using Domain.Auth.Constants;
using FluentValidation;
using Presentation.Users.DataTransferObjects;

namespace Presentation.Users.Validators
{
    public class ManageRolesRequestBodyValidator : AbstractValidator<ManageRolesRequestBody>
    {
        private readonly List<string> _roles = [Role.Admin, Role.Moderator, Role.User];

        public ManageRolesRequestBodyValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(RoleIsValid)
                .WithMessage($"Role is not valid and must be one of these: {string.Join("", _roles.Select((role, index) => index + 1 == _roles.Count ? $"{role}" : $"{role}, "))}.");
        }

        private bool RoleIsValid(string role)
        {
            return _roles.Contains(role);
        }
    }
}
