using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    UserName = registerVM.Email.Split('@')[0],
                    Email = registerVM.Email,
                    IsAgree = registerVM.IsAgree,

                };

                var result = await userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Login));
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(registerVM);
        }


        public IActionResult Login()
        {
            return View();
            
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(loginVM.Email);
                if (user != null)
                {
                    var password = await userManager.CheckPasswordAsync(user, loginVM.Password);
                    if(password)
                    {
                        var result = await signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(loginVM);

        }

        public async new Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
