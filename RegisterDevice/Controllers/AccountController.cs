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


        /* forgot password */
        [HttpGet]
       public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) {

                return RedirectToAction("ForgotPasswordConfirmation");
            
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = Url.Action(
                
                "ResetPassword",
                "Account",
                new {token, email= model.Email},
                Request.Scheme);


            TempData["ResetLink"] = resetLink;
                
            return RedirectToAction("ForgotPasswordConfirmation");
        }


       public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
                return RedirectToAction("Login");


            var model = new ResetPasswordViewModel
            {
                Token = token,
                Email = email

            };
            return View(model);
        }




        [HttpPost]
        public  async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction("Login");

            var result = await _userManager.ResetPasswordAsync(
                user,
                model.Token,
                model.Password);

            if (result.Succeeded)
                return RedirectToAction("Login");
            foreach( var error in result.Errors)
                ModelState.AddModelError("",error.Description);


           return View(model); 

        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword

                );
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("ChangePasswordConfirmation");

            }
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
        public IActionResult ChangePasswordConfirmation()
        {
            return View();
        }

    }
}
