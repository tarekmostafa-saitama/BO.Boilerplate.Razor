using Application.Requests.Roles.Commands;
using Application.Requests.Roles.Models;
using Application.Requests.Roles.Queries;
using FormHelper;
using Infrastructure.Identity.PermissionHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NToastNotify;
using Shared.Permissions;

namespace UI.Razor.Areas.Admin.Controllers;

[Area("Admin")]
public class RolesController : Controller
{
    private readonly ISender _sender;
    private readonly IStringLocalizer<RolesController> _stringLocalizer;
    private readonly IToastNotification _toastNotification;

    public RolesController(
        IStringLocalizer<RolesController> stringLocalizer,
        IToastNotification toastNotification,
        ISender sender
    )
    {
        _stringLocalizer = stringLocalizer;
        _toastNotification = toastNotification;
        _sender = sender;
    }

    [HttpGet("Dashboard/Roles")]
    [MustHavePermission(Actions.View, Resources.Roles)]
    public async Task<IActionResult> List()
    {
        var roles = await _sender.Send(new GetRolesQuery());
        return View(roles);
    }

    [HttpGet("Dashboard/Roles/Create")]
    [MustHavePermission(Actions.Create, Resources.Roles)]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Dashboard/Roles/Create")]
    [MustHavePermission(Actions.Create, Resources.Roles)]
    [FormValidator]
    public async Task<IActionResult> Create(RoleVm roleVm)
    {
        var result = await _sender.Send(new CreateRoleCommand(roleVm));
        if (result.Succeeded)
            return FormResult.CreateSuccessResult(_stringLocalizer["savedSuccess"], Url.Action(nameof(List)));
        return FormResult.CreateErrorResult(result.Errors.First());
    }

    [HttpGet("Dashboard/Roles/{roleId}/Update")]
    [MustHavePermission(Actions.Update, Resources.Roles)]
    public async Task<IActionResult> Update(string roleId)
    {
        var result = await _sender.Send(new GetRoleQuery(roleId, true));

        //TODO: To Refactor this mess later
        ViewBag.ExitingPermissions = result.Permissions;
        result.Permissions =
            new List<string>(new string[Permissions.CategorizedPermissions.SelectMany(x => x.Value).Count()]);
        return View(result);
    }

    [HttpPost("Dashboard/Roles/{roleId}/Update")]
    [MustHavePermission(Actions.Update, Resources.Roles)]
    [FormValidator]
    public async Task<IActionResult> Update(string roleId, RoleVm roleVm)
    {
        //TODO: To Refactor this mess later
        roleVm.Permissions = roleVm.Permissions.Where(x => x != "false").ToList();
        var result = await _sender.Send(new UpdateRoleCommand(roleVm));
        if (result.Succeeded)
            return FormResult.CreateSuccessResult(_stringLocalizer["savedSuccess"], Url.Action(nameof(List)));
        return FormResult.CreateErrorResult(result.Errors.First());
    }

    [HttpPost("Dashboard/Roles/{roleId}/Delete")]
    [MustHavePermission(Actions.Delete, Resources.Roles)]
    public async Task<IActionResult> Delete(string roleId)
    {
        await _sender.Send(new DeleteRoleCommand(roleId));
        _toastNotification.AddSuccessToastMessage(_stringLocalizer["deleteSuccessfully"]);
        return RedirectToAction(nameof(List));
    }
}