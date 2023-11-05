using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Requests.Roles.Models;

public class RoleVmValidator : AbstractValidator<RoleVm>
{
    public RoleVmValidator(IRoleService roleService, IStringLocalizer<RoleVmValidator> localizer)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["requiredField"]);
    }
}