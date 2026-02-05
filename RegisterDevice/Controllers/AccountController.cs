using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegisterDevice.Data;
using RegisterDevice.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RegisterDevice.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController (ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private async Task<string> GenerateUniqueUsername(string fullname)
        {
            string baseUsername;
            string username;

            var parts = fullname.Trim().Split(' ');
            string firstInitial = parts[0][0].ToString().ToLower();
            string lastName = parts[^1].ToLower();

            do
            {
                int number = Random.Shared.Next(100, 999);
                baseUsername = $"{firstInitial}.{lastName}";
                username = $"{baseUsername}_{number}";
            }
            while (await _userManager.FindByNameAsync(username) != null);

            return username;
        }





        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> Register(RegisterViewModel model)
          {

            if (!ModelState.IsValid) return View(model);

           var  userName= await GenerateUniqueUsername(model.FullName);

            var user = new ApplicationUser
            {
                UserName=userName,
                Email=model.Email,
                PhoneNumber=model.PhoneNumber,
                FullName=model.FullName
               

            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
         }


          public IActionResult Success()
         {
              return View();
          }




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginViewModel model) {

            if (!ModelState.IsValid) return View(model);

            ApplicationUser user;

            if (model.Login.Contains("@"))
            { 
            user= await _userManager.FindByEmailAsync(model.Login);
            
            }
            else { 
            user= await _userManager.FindByNameAsync(model.Login);
            
            }

            if (user == null) {

                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                model.Password,
                false,
                false

                );

            if (!result.Succeeded) {

                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            
            }
           
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Sign out logic here
           await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }



       

    }
}
