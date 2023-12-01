using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Common.Models.UserModels;

public class CreateUserValidator : AbstractValidator<CreateUserVm>
{

    public CreateUserValidator(IUserService userService, IStringLocalizer<CreateUserVm> localizer)
    {
        RuleFor(x => x.FullName).NotEmpty().WithMessage(localizer["requiredField"]);

        RuleFor(x => x.Email).NotEmpty().WithMessage(localizer["requiredField"]);
        RuleFor(x => x.Email).EmailAddress().WithMessage(localizer["notValidEmail"]);
        RuleFor(x => x.Email).Must( (x,context) => !userService.IsUserExistByEmailAsync(x.Email).Result)
            .WithMessage(localizer["emailUsedBefore"]);

        RuleFor(x => x.Password).MinimumLength(6).WithMessage(localizer["minLengthField",5]);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage(localizer["notEqualPassword"]);
    }
}