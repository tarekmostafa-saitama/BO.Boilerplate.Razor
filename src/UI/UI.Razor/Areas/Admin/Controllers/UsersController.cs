using Application.Common.Models.UserModels;
using Application.Requests.UsersManagement.Commands;
using Application.Requests.UsersManagement.Queries;
using FormHelper;
using Infrastructure.Identity.PermissionHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult List()
    {
        return View();
    }

    [HttpPost("Dashboard/Users/Paginate")]
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
    public IActionResult Create()
    {
        return View(new CreateUserVm());
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
    public IActionResult Update()
    {
        return View();
    }


    [HttpGet("Dashboard/Users/{userId}/Delete")]
    public IActionResult Delete()
    {
        return View();
    }
}