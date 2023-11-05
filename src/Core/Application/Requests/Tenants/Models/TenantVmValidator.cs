using System.Security.Cryptography.X509Certificates;
using Application.Common.Tenants;
using Application.Repositories;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Requests.Tenants.Models;

public class TenantVmValidator :AbstractValidator<TenantVm>
{
    public TenantVmValidator(IUnitOfWork unitOfWork ,IStringLocalizer<TenantVmValidator> localizer)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["requiredField"]);
        RuleFor(x => x.Name).Must(predicate: (dto, name, context) =>
        {
            if (dto.Id == default)
            {
                var result =
                    unitOfWork.TenantsRepository.GetSingleAsync( i=> i.Name == name).Result == null;
                return result; 
            }
            else
            {
                var result =
                    unitOfWork.TenantsRepository.GetSingleAsync(i => i.Id!=dto.Id &&  i.Name == name).Result == null;
                return result;
            }
        }).WithMessage(localizer["nameUsedBefore"]); ;


    }

 
}