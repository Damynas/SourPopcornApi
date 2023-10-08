using FluentValidation;
using Presentation.Users.DataTransferObjects;

namespace Presentation.Users.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequestBody>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required.")
            .MaximumLength(20).WithMessage("Display name cannot exceed 20 characters.");
    }
}
