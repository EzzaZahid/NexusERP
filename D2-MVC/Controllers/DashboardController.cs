
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NexusERP_D2.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View("Admin");
            }

            if (User.IsInRole("Employee"))
            {
                return View("Employee");
            }

            return Forbid();
        }
    }
}
