using Application.Requests.Trails.Queries;
using DocumentFormat.OpenXml.Office2010.Excel;
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

    [HttpGet("Dashboard/Transactions/{id}/ViewPartial")]
    [MustHavePermission(Actions.View, Resources.Transactions)]
    public async Task<IActionResult> ViewPartial(int id)
    {
        var result = await _sender.Send(new GetTrailQuery(id)); 
        return PartialView(result);
    }
}