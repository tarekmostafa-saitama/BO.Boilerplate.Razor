using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Requests.Users.Models;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator(IStringLocalizer<LoginUserRequestValidator> stringLocalizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(stringLocalizer["requiredField"]);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(stringLocalizer["requiredField"])
            .MinimumLength(6).WithMessage("djkdsio");
    }
}