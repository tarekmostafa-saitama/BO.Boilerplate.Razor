using Application.Common.Models.UserModels;
using Application.Requests.Roles.Commands;
using Application.Requests.Roles.Models;
using Application.Requests.Roles.Queries;
using Application.Requests.Tenants.Queries;
using Application.Requests.Users.Commands;
using Application.Requests.Users.Models;
using Application.Requests.Users.Queries;
using Application.Requests.UsersManagement.Commands;
using Application.Requests.UsersManagement.Queries;
using DocumentFormat.OpenXml.Spreadsheet;
using FormHelper;
using Hangfire;
using Infrastructure.Identity;
using Infrastructure.Identity.PermissionHandlers;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using NToastNotify;
using Shared.Extensions;
using Shared.Models.PaginateModels;
using Shared.Permissions;

namespace UI.Razor.Areas.Admin.Controllers;

[Area("Admin")]
public class UsersController : Controller
{
    private readonly ISender _sender;
    private readonly IStringLocalizer<UsersController> _stringLocalizer;
    private readonly IToastNotification _toastNotification;


    public UsersController(ISender sender, IStringLocalizer<UsersController> stringLocalizer,
        IToastNotification toastNotification)
    {
        _sender = sender;
        _stringLocalizer = stringLocalizer;
        _toastNotification = toastNotification;
    }


    [HttpGet("Dashboard/Users")]
    [MustHavePermission(Actions.View, Resources.Users)]
    public IActionResult List()
    {
        return View();
    }

    [HttpPost("Dashboard/Users/Paginate")]
    [MustHavePermission(Actions.View, Resources.Users)]
    public async Task<IActionResult> PaginateUsers(DateTime? from, DateTime? to)
    {
        var dataTableModel = new PaginateModel<string>
        {
            Request = HttpContext.Request.MapToRequestModel(),
            FilteredById = null,
            From = from,
            To = to
        };
        var model = await _sender.Send(new GetUsersQuery(dataTableModel));
        return Ok(model);
    }

    [HttpGet("Dashboard/Users/Create")]
    [MustHavePermission(Actions.Create, Resources.Users)]
    public async Task<IActionResult> Create()
    {
        var roles = await _sender.Send(new GetRolesQuery());
        ViewBag.Roles = roles.Select(x=>x.Name).ToList();
        var tenants = await _sender.Send(new GetTenantsQuery(true));
        ViewBag.Tenants = new SelectList(tenants, "Id", "Name");

        return View();
    }

    [HttpPost("Dashboard/Users/Create")]
    [MustHavePermission(Actions.Create, Resources.Users)]
    [FormValidator]
    public async Task<IActionResult> Create(CreateUserVm createUserVm)
    {
        var result = await _sender.Send(new CreateUserCommand(createUserVm));
        if (result.Succeeded)
            return FormResult.CreateSuccessResult(_stringLocalizer["savedSuccess"], Url.Action(nameof(List)));
        return FormResult.CreateErrorResult(result.Errors.First());
    }



    [HttpGet("Dashboard/Users/{userId}/Update")]
    [MustHavePermission(Actions.Update, Resources.Users)]
    public async Task<IActionResult> Update(string userId)
    {
        //TODO: To Refactor this mess later
        var user = await _sender.Send(new GetUserQuery(userId,true));
        ViewBag.UserRoles = user.RoleVms.Select(x=>x.Name).ToList();

        var roles = await _sender.Send(new GetRolesQuery());
        ViewBag.Roles = roles.Select(x => x.Name).ToList();

        var tenants = await _sender.Send(new GetTenantsQuery(true));
        ViewBag.Tenants = new SelectList(tenants, "Id", "Name", user.TenantId);

        user.RoleVms =   new List<RoleVm>(new RoleVm[roles.Count]);
        return View(user.Adapt<UpdateUserVm>());
    }

    [HttpPost("Dashboard/Users/{userId}/Update")]
    [MustHavePermission(Actions.Update, Resources.Users)]
    [FormValidator]
    public async Task<IActionResult> Update(string userId, UpdateUserVm userVm)
    {
        //TODO: To Refactor this mess later

        userVm.RoleVms = userVm.RoleVms.Where(x => x.Name != "false").ToList();
        var result = await _sender.Send(new UpdateUserCommand(userVm));
        if (result.Succeeded)
            return FormResult.CreateSuccessResult(_stringLocalizer["savedSuccess"], Url.Action(nameof(List)));
        return FormResult.CreateErrorResult(result.Errors.First());
    }


    [HttpPost("Dashboard/Users/{userId}/Delete")]
    [MustHavePermission(Actions.Delete, Resources.Users)]   
    public async Task<IActionResult> Delete(string userId)
    {
        await _sender.Send(new DeleteUserCommand(userId));
        _toastNotification.AddSuccessToastMessage(_stringLocalizer["deleteSuccessfully"]);
        return RedirectToAction(nameof(List));
    }


    [HttpGet("Dashboard/Users/{userId}/FastLogin")]
    public async Task<IActionResult> FastLogin(string userId)
    {

        var result = await _sender.Send(new FastLoginQuery(userId));
        if (!result.Succeeded)
        {
          _toastNotification.AddErrorToastMessage(result.Errors[0]);
          throw new Exception(); 
        }

        _toastNotification.AddSuccessToastMessage(_stringLocalizer["loginPage_successLogin"].Value);
        return RedirectToAction("Home", "Home");

    }


    [HttpGet("Dashboard/Users/{userId}/AlterUserPasswordPartial")]
    public async Task<IActionResult> AlterUserPasswordPartial(string userId)
    {
        return PartialView(new AlterPasswordVm(){UserId = userId});
    }


    [HttpPost("Dashboard/Users/{userId}/AlterUserPasswordPartial")]
    [FormValidator]
    public async Task<IActionResult> AlterUserPasswordPartial(AlterPasswordVm changePassword , string userId)
    {
        var result = await _sender.Send(new AlterPasswordCommand(changePassword)); 

        if (result.Succeeded)
            return FormResult.CreateSuccessResult(_stringLocalizer["savedSuccess"], Url.Action(nameof(List)));
        return FormResult.CreateErrorResult(result.Errors.First());
    }

    [HttpGet("Dashboard/Users/{userId}/ToggleUserActivation")]
    public async Task<IActionResult> ToggleUserActivation(string userId)
    {
        var result = await _sender.Send(new ToggleUserActivationQuery(userId));

        if (!result.Succeeded)
        {
            _toastNotification.AddErrorToastMessage(result.Errors[0]);
            throw new Exception();
        }
        return RedirectToAction(nameof(List));
    }
}