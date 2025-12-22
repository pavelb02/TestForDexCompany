using FluentValidation;

namespace Domain.Validation;

public class ImageValidator : AbstractValidator<string>
{
    public ImageValidator()
    {
        RuleFor(x => x)
            .Matches(@"\.(png|jpe?g)$")
            .When(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Файл аватарки должен быть с расширением .jpg, .jpeg или .png.");
    }
}