using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Razor.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class HomeController : Controller
{
    [HttpGet("Dashboard/Home")]
    public async Task<IActionResult> Home()
    {
        return View();
    }
}