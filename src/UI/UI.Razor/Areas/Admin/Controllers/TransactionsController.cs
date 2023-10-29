using Infrastructure.Identity.PermissionHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Permissions;

namespace UI.Razor.Areas.Admin.Controllers;

[Area("Admin")]
public class TransactionsController : Controller
{
    private readonly ISender _sender;

    public TransactionsController(ISender sender)
    {
        _sender = sender;
    }
    [HttpGet("Dashboard/")]
    [MustHavePermission(Actions.View, Resources.Transactions)]
    public IActionResult List()
    {
        return View();
    }
}