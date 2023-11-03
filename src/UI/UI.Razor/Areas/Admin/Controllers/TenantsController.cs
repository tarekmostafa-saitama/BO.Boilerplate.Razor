using Application.Requests.Tenants.Commands;
using Application.Requests.Tenants.Models;
using Application.Requests.Tenants.Queries;
using FormHelper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NToastNotify;

namespace UI.Razor.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TenantsController : Controller
    {
        private readonly ISender _sender;
        private readonly IStringLocalizer<TenantsController> _stringLocalizer;
        private readonly IToastNotification _toastNotification;


        public TenantsController(ISender sender, IStringLocalizer<TenantsController> stringLocalizer, IToastNotification toastNotification)
        {
            _sender = sender;
            _stringLocalizer = stringLocalizer;
            _toastNotification = toastNotification;
        }
        [HttpGet("Dashboard/Tenants")]
        public async Task<IActionResult> List()
        {
            var tenants =  await _sender.Send(new GetTenantsQuery()); 

            return View(tenants);
        }

        [HttpGet("Dashboard/Tenants/{id}/Set")]
        public async Task<IActionResult> SetPartial(Guid id)
        {
            var tenantVm = id == default ? new TenantVm() : await _sender.Send(new GetTenantQuery(id));
            return PartialView(tenantVm);
        }

        [HttpPost("Dashboard/Tenants/{id}/Set")]
        [FormValidator]
        public async Task<IActionResult> SetPartial(Guid id ,TenantVm tenantVm)
        {
            await _sender.Send(new SetTenantCommand(tenantVm));
            return FormResult.CreateSuccessResult(_stringLocalizer["savedSuccess"], Url.Action(nameof(List)));
        }

        [HttpPost("Dashboard/Tenants/{id}/Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _sender.Send(new DeleteTenantCommand(id));
            _toastNotification.AddSuccessToastMessage(_stringLocalizer["deleteSuccessfully"]);
            return RedirectToAction(nameof(List));
        }
    }
}
