using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Requests.Users.Models;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator(IStringLocalizer<LoginUserRequestValidator> localizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["requiredField"]);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer["requiredField"])
            .MinimumLength(6).WithMessage(localizer["minLengthField" , 6]);
    }
}