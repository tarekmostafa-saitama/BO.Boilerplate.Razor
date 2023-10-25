using Microsoft.AspNetCore.Mvc;

namespace UI.Razor.Areas.Admin.Controllers;

[Area("Admin")]
public class TransactionsController : Controller
{
    public IActionResult List()
    {
        return View();
    }
}