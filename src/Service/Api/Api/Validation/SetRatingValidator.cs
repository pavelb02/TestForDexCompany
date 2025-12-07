using Application.Services.DTO;
using FluentValidation;

namespace Api.Validation;

public class SetRatingValidator: AbstractValidator<RatingRequest>
{
    public SetRatingValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Рейтинг может быть в диапазоне от 1 од 5.");
    }
}