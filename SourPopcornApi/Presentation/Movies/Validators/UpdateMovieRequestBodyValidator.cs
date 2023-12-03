using FluentValidation;
using Presentation.Movies.DataTransferObjects;
using System.Globalization;

namespace Presentation.Movies.Validators;

public class UpdateMovieRequestBodyValidator : AbstractValidator<UpdateMovieRequestBody>
{
    public UpdateMovieRequestBodyValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 20 characters.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.");
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(20).WithMessage("Country cannot exceed 20 characters.");
        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("Language is required.")
            .MaximumLength(20).WithMessage("Language cannot exceed 20 characters.");
        RuleFor(x => x.ReleasedOn)
                .NotEmpty().WithMessage("Released date is required.")
                .Must(DateIsValid).WithMessage("Invalid date. Please follow this format: 'YYYY-MM-DD'.");
    }

    private static bool DateIsValid(string dateString)
    {
        return DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }
}
