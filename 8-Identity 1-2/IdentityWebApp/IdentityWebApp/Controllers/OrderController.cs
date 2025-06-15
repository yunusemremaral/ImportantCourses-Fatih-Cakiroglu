using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class OrderController : Controller
    {


        
       // [Authorize(Policy = "OrderPermissionReadAndDelete")]

        [Authorize(Policy = "Permissions.Order.Read")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
