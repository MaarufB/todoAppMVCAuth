using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp_MVC.Data;
using TodoApp_MVC.Models;
using TodoApp_MVC.ViewModels.Account;

namespace TodoApp_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDataContext _context;
        public AccountController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            ApplicationDataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        [HttpGet]
        public IActionResult Register()
        {
            var response = new RegisterViewModel();

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager
                        .FindByEmailAsync(registerViewModel.EmailAddress);

            if (user != null)
            {
                TempData["Error"] = "This email address is already in use.";
                return View(registerViewModel);
            }

            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress
            };

            var newUserResponse = await _userManager
                                    .CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            var loginResponse = new LoginViewModel();

            return View(loginResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                // user is found check password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

                if (passwordCheck)
                {

                    // Password correct, Sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
            }

            // Password is incorrect
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

    }
}
