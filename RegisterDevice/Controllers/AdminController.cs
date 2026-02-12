using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using RegisterDevice.Data;
using RegisterDevice.Models;

namespace RegisterDevice.Controllers
{
    public class AdminController:Controller
    {


        private const string ADMIN_EMAIL = "admin@system.com";
        private const string ADMIN_PASSWORD = "admin123";
        private readonly ApplicationDbContext _context;

        public AdminController(
          ApplicationDbContext context)
        {
            _context = context;
        
        }

        public IActionResult Dashboard()
        {

            var guard = AdminOnly();
            if (guard != null) return guard;


            var model = new AdminDashboardViewModel
            {

                TotalDevices = _context.MyDevices.Count(),
                TotalUser = _context.Users.Count(),
                TotalLostReports = _context.LostDevicesReports.Count(r => !r.IsResolved),


                RecentLostDevices = _context.LostDevicesReports
                    .Where(r => !r.IsResolved)
                    .OrderByDescending(r => r.ReportedAt)
                    .Take(5)
                    .ToList(),



                RecentDevies= _context.MyDevices
                     .OrderByDescending(d => d.RegisteredAt)
                     .Take(5)
                     .ToList(),


                RecentUser= _context.Users
                    .OrderByDescending(u => u.Id)
                    .Take(5)
                    .ToList(),


            };
            return View(model);

        }

        public IActionResult LostDevices()
        {


            var guard = AdminOnly();
            if (guard != null) return guard;

            var reports = _context.LostDevicesReports
                .OrderByDescending(r => r.ReportedAt)
                .ToList();

            return View(reports);

        }


        public IActionResult RegisteredDevices()
        {

            var guard = AdminOnly();
            if (guard != null) return guard;


            return View(_context.MyDevices.ToList());


        }

        public IActionResult RegisteredAccounts()
        {


            var guard = AdminOnly();
            if (guard != null) return guard;

            return View(_context.Users.ToList());
        }

        public IActionResult Login() => View ();

        [HttpPost]
        public IActionResult Login(AdminLoginViewModel model)
        {
            if(!ModelState.IsValid)
                return View (model);

            if (model.Email == ADMIN_EMAIL && model.Password == ADMIN_PASSWORD)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "Invalid admin credentials");

            return View (model);

        } 


        public IActionResult Logout()
        {


            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Login");
        }


        private bool IsAdminLoggedIn()
        {

            return HttpContext.Session.GetString("IsAdmin")=="true";
        }

        private IActionResult AdminOnly()
        {


            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");


            return null;
        }



    }
}
