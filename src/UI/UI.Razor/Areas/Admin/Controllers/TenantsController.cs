using Microsoft.AspNetCore.Mvc;

namespace UI.Razor.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TenantsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
