using Application.Services.DTO;
using FluentValidation;

namespace Api.Validation;

public class UpdateAdvertisementRequestValidator : AbstractValidator<UpdateAdvertisementRequest>
{
    public UpdateAdvertisementRequestValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Текст обязателен")
            .MaximumLength(2000).WithMessage("Максимальная длина текста — 2000 символов");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Дата окончания обязательна.");    }
}