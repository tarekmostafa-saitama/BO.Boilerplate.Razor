using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Requests.Roles.Models;

public class RoleVmValidator : AbstractValidator<RoleVm>
{
    public RoleVmValidator(IRoleService roleService)
    {
    }
}