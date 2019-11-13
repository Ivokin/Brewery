using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Brewery.Core.Constants;

namespace Brewery.Web.Controllers
{
    [Authorize(Roles = AccountConstants.AdminRole)]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewData[AccountConstants.AdminName] = HttpContext.User.Identity.Name;
            return View();
        }
    }
}
