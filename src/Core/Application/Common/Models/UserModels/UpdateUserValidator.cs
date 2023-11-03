using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Common.Models.UserModels;

public class UpdateUserValidator: AbstractValidator<UpdateUserVm>
{
    public UpdateUserValidator(IUserService userService, IStringLocalizer<CreateUserVm> localizer)
    {
        RuleFor(x => x.FullName).NotEmpty().WithMessage(localizer["requiredField"]);
    }
}