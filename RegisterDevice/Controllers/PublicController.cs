using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegisterDevice.Data;

namespace RegisterDevice.Controllers
{
    public class PublicController : Controller
    {

        private readonly ApplicationDbContext _context; 


        public PublicController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult CheckDevice()
        {
           
            return View();
        }

         [HttpPost]
         public async Task<IActionResult> CheckDevice(string identifier)
        {

            if(string.IsNullOrWhiteSpace(identifier))
            {
               ModelState.AddModelError("", "Please enter an IMEI or Serial Number.");
               return View();
            }

            var lostDevice = await _context.LostDevicesReports
                .Include(r => r.Device)
                .Where(r => r.Identifier == identifier && !r.IsResolved)
                .OrderByDescending(r => r.ReportedAt)
                .FirstOrDefaultAsync();

               
            if(lostDevice == null)
            {
                ViewBag.NotFound = true;
                return View();
               
            }
           


            return View( "CheckResult", lostDevice);
        }

    }
}
