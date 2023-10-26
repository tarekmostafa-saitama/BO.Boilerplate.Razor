using Application.Common.Models.UserModels;
using Application.Requests.UsersManagement.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NToastNotify;
using Shared.Extensions;
using Shared.Models.PaginateModels;

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

    public IActionResult Create()
    {
        return View(new CreateUserVm());
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