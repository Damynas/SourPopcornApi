using FluentValidation;
using Presentation.Movies.DataTransferObjects;

namespace Presentation.Movies.Validators;

public class CreateMovieRequestValidator : AbstractValidator<CreateMovieRequestBody>
{
    public CreateMovieRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.");
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(20).WithMessage("Country cannot exceed 20 characters.");
        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("Language is required.")
            .MaximumLength(20).WithMessage("Language cannot exceed 20 characters.");
    }
}
