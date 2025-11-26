using FluentValidation;

namespace Domain.Validation;

public class ImageValidator : AbstractValidator<string>
{
    public ImageValidator()
    {
        RuleFor(x => x)
            .Must(x => string.IsNullOrEmpty(x) || x.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || x.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || x.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Файл аватарки должен быть с расширением .jpg, .jpeg или .png.");
    }
}