using Microsoft.AspNetCore.Mvc;

namespace RegisterDevice.Controllers
{
    public class PublicController : Controller
    {
        public IActionResult CheckDevice()
        {
           
            return View();
        }
    }
}
