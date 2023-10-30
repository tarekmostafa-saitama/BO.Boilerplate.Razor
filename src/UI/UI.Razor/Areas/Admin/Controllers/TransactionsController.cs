using Application.Requests.Trails.Queries;
using Infrastructure.Identity.PermissionHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using Shared.Models.PaginateModels;
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
    [HttpGet("Dashboard/Transactions")]
    [MustHavePermission(Actions.View, Resources.Transactions)]
    public IActionResult List()
    {
        return View();
    }

    [HttpPost("Dashboard/Transactions/Paginate")]
    [MustHavePermission(Actions.View, Resources.Transactions)]
    public async Task<IActionResult> PaginateTransactions(DateTime? from, DateTime? to)
    {
        var dataTableModel = new PaginateModel<string>
        {
            Request = HttpContext.Request.MapToRequestModel(),
            FilteredById = null,
            From = from,
            To = to
        };
        var model = await _sender.Send(new GetTrailsQuery(dataTableModel));
        return Ok(model);
    }
}