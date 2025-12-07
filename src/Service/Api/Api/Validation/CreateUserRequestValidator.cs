using Application.Services.DTO;
using FluentValidation;

namespace Api.Validation;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя пользователя обязательно.")
            .MinimumLength(2).WithMessage("Имя должно состоять минимум из 2-х символов.")
            .MaximumLength(30).WithMessage("Имя не должно превышать 30 символов.");
    }
}