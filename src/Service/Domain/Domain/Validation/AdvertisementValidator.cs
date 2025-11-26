using Domain.Entities;
using FluentValidation;

namespace Domain.Validation;

public class AdvertisementValidator: AbstractValidator<Advertisement>
{
    public AdvertisementValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Текст объявления обязателен.")
            .MaximumLength(2000).WithMessage("Текст не должен превышать 2000 символов.");
        
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("Дата окончания должна быть позже или равна дате начала объявления.");
    }
}