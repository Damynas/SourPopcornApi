using FluentValidation;
using Presentation.Ratings.DataTransferObjects;

namespace Presentation.Ratings.Validators;

public class CreateRatingRequestValidator : AbstractValidator<CreateRatingRequestBody>
{
    public CreateRatingRequestValidator()
    {
        RuleFor(x => x.SourPopcorns)
            .GreaterThanOrEqualTo(0).WithMessage("SourPopcorns must be an integer from 0 to 5.")
            .LessThanOrEqualTo(5).WithMessage("SourPopcorns must be an integer from 0 to 5.");
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required.")
            .MaximumLength(200).WithMessage("Comment cannot exceed 200 characters.");
    }
}
