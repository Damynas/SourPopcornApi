using Application.Users.Abstractions;
using Domain.Users.DataTransferObjects.Requests;
using FluentValidation;
using Presentation.Users.DataTransferObjects;

namespace Presentation.Users.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequestBody>
{
    private readonly IUserService _userService;

    public CreateUserRequestValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(20).WithMessage("Username cannot exceed 20 characters.")
            .MustAsync(UsernameIsUnique).WithMessage("Username is already in use.");
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

    }

    private async Task<bool> UsernameIsUnique(string username, CancellationToken cancellationToken)
    {
        var request = new GetUserByUsernameRequest(username);
        var result = await _userService.GetUserByUsernameAsync(request, cancellationToken);
        return result.IsFailure;
    }
}
