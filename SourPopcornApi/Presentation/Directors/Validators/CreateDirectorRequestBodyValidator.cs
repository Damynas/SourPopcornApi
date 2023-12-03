using FluentValidation;
using Presentation.Directors.DataTransferObjects;
using System.Globalization;

namespace Presentation.Directors.Validators;

public class CreateDirectorRequestBodyValidator : AbstractValidator<CreateDirectorRequestBody>
{
    public CreateDirectorRequestBodyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(20).WithMessage("Name cannot exceed 20 characters.");
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(20).WithMessage("Country cannot exceed 20 characters.");
        RuleFor(x => x.BornOn)
            .NotEmpty().WithMessage("Born date is required.")
            .Must(DateIsValid).WithMessage("Invalid date. Please follow this format: 'YYYY-MM-DD'.");
    }

    private static bool DateIsValid(string dateString)
    {
        return DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }
}
