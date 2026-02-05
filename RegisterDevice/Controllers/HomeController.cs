using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegisterDevice.Data;
using RegisterDevice.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace RegisterDevice.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public HomeController(
          ApplicationDbContext context,
          IWebHostEnvironment environment,
          UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }



        public IActionResult LandingPage()
        {
            if(User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");

            }
            return View();
        }


        [Authorize]
        public IActionResult Index()
        {
            var devices = _context.MyDevices
                .Include(d => d.DeviceImages)
                .Where(d => d.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .OrderByDescending(d => d.RegisteredAt)
                .ToList();
          
            return View( devices);
        }
     

        public IActionResult Details()
        {

            return View();
        }

        [Authorize]
        [HttpGet]
       public IActionResult EditDevice(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var device = _context.MyDevices
                .Include (d => d.DeviceImages)
                .FirstOrDefault(d =>d.Id  == id && d.UserId==userId);

            if(device == null)
                return NotFound();

            return View(device);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDevice(
            
            int id,
            RegisteredDevice updatedDevice,
            List<IFormFile> DeviceImages)
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            var device = _context.MyDevices
                .Include (d => d.DeviceImages)
                .FirstOrDefault(d => d.Id == id && d.UserId==userId);


            if(device==null)
                return NotFound();
            
            if(!ModelState.IsValid)
                return View(device);

            device.DeviceType = updatedDevice.DeviceType;
            device.Brand = updatedDevice.Brand;
            device.Model = updatedDevice.Model;
            device.Notes = updatedDevice.Notes;





            if(DeviceImages != null && DeviceImages.Count > 0)
            {


                string uploadFolder = Path.Combine(_environment.WebRootPath,

                    "upload",
                    "devices",
                    device.Id.ToString()

                    );



                Directory.CreateDirectory(uploadFolder );

                foreach(var image in DeviceImages)
                {
                  if(image.Length==0) continue;

                    string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await image.CopyToAsync(stream);


                    device.DeviceImages.Add(new DeviceImage
                    {

                        ImagePath = $"/upload/devices/{device.Id}/{fileName}",
                        UploadedAt = DateTime.Now

                    });

                }    
            }

            await _context.SaveChangesAsync();

            TempData["success"] = "Device updated Successfully";

            return RedirectToAction("Index", new { id = device.Id });
        }




        // Register Device
        [Authorize]
        [HttpGet]
        public IActionResult RegisterDevice()
        {
            
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterDevice(
            RegisteredDevice device,
                List<IFormFile> DeviceImages)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool exists = _context.MyDevices.Any(d => d.Identifier == device.Identifier);


            if (exists)
            {
                ModelState.AddModelError("Identifier", "This IMEI / Serial Number is already registered.");
                return View(device);

            }
            device.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            device.RegisteredAt= DateTime.Now;
            device.Status = "Active";

            _context.MyDevices.Add(device);
            await _context.SaveChangesAsync();

            // Image uploads

            if (DeviceImages != null && DeviceImages.Count > 0)
            {


                string uploadFolder = Path.Combine(

                  _environment.WebRootPath,
                  "upload",
                  "devices",
                  device.Id.ToString()

                    );

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);

                }

                foreach(var image in DeviceImages)
                {
                    if (image==null || image.Length ==0)
                        continue;

                    string fileName = Guid.NewGuid().ToString()
                        + Path.GetExtension(image.FileName);

                    string filePath= Path.Combine(uploadFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {

                        await image.CopyToAsync(stream);
                    }
                    device.DeviceImages.Add(new DeviceImage
                    {

                        ImagePath = $"/upload/devices/{device.Id}/{fileName}",
                        UploadedAt=DateTime.Now,



                    });
                }
               await _context.SaveChangesAsync();
            }

            TempData["sucess"] = "Device registerd sucessfully";


            return RedirectToAction("Index");
        }









        // Device Details 
        [Authorize]
        public IActionResult DeviceDetails( int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var device = _context.MyDevices
                .Include(d => d.DeviceImages)
                .Include(d =>d.User)
                .FirstOrDefault(d => d.Id == id && d.UserId == userId);




           if (device == null)
            {
                return NotFound();
            }


            return View(device);
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkLost(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var device = _context.MyDevices                
                .FirstOrDefault(d => d.Id == id && d.UserId == userId);

            if (device == null)
                return BadRequest("Device not found");

            if (device.Status == "Lost")
                return RedirectToAction("Index");

            device.Status = "Lost";

            var lostReport = new LostDeviceReport
            {

                DeviceId = device.Id,
                Identifier = device.Identifier,
                Brand=device.Brand,
                Model=device.Model,
                ReportedAt=DateTime.Now,
                ReportedByUserId=userId,
                IsResolved=false



            };
            _context.LostDevicesReports.Add(lostReport);

            await _context.SaveChangesAsync();

            TempData["success"] = "Device marked as lost.";

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkFound(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var device = await _context.MyDevices
                .FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);

            if (device == null)
                return NotFound();

            device.Status = "Active";

            var report = await _context.LostDevicesReports
                .Where(r => r.DeviceId==id && !r.IsResolved)
                .OrderByDescending(r => r.ReportedAt)
                .FirstOrDefaultAsync();

            if (report != null)
                report.IsResolved = true;

            await _context.SaveChangesAsync();

            TempData["success"] = "Device marked as found.";

            return RedirectToAction(nameof(Index));
        }





        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");
            var model = new ProfileViewModel
            {

                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,

                TotalDevices = _context.MyDevices.Count(d => d.UserId== user.Id),
                LostDevices = _context.MyDevices.Count(d => d.UserId ==user.Id && d.Status=="Lost"),

            };

              return View(model);  
         }


    }
}
