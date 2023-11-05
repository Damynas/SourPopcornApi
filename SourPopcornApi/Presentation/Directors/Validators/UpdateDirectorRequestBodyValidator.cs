using FluentValidation;
using Presentation.Directors.DataTransferObjects;

namespace Presentation.Directors.Validators;

public class UpdateDirectorRequestBodyValidator : AbstractValidator<UpdateDirectorRequestBody>
{
    public UpdateDirectorRequestBodyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(20).WithMessage("Name cannot exceed 20 characters.");
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(20).WithMessage("Country cannot exceed 20 characters.");
    }
}
