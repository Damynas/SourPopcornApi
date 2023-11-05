using FluentValidation;
using Presentation.Auth.DataTransferObjects;

namespace Presentation.Auth.Validators;

public class LoginRequestBodyValidator : AbstractValidator<LoginRequestBody>
{
    public LoginRequestBodyValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
