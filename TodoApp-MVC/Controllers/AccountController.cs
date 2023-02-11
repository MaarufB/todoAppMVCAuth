using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using TodoApp_MVC.Data;
using TodoApp_MVC.Models;
using TodoApp_MVC.Repositories.UserRepo;
using TodoApp_MVC.ViewModels.Account;

namespace TodoApp_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDataContext _context;
        private readonly IUserRepository _userRepository;

        public AccountController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            ApplicationDataContext context,
            IUserRepository userRepository)
        
        {
            _userRepository = userRepository;
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

            var registerUser = await _userRepository.RegisterAsync(registerViewModel);
            if (!registerUser.IsSuccess)
            {
                TempData["Error Message"] = registerUser.Message;
                return View(registerViewModel);
            }

            // login the user automatically
            var login = new LoginViewModel
            {
                EmailAddress = registerViewModel.EmailAddress,
                Password = registerViewModel.Password
            };

            var loginResponse = await _userRepository.LoginAsync(login);
            if (!loginResponse.IsSuccess)
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("Index", "Todo");
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

            var user = await _userRepository.FindUserByEmailAsync(loginViewModel.EmailAddress);
            
            if (user == null)
            {
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginViewModel);
            }

            var login = await _userRepository.LoginAsync(loginViewModel);
            if (!login.IsSuccess)
            {
                TempData["Error"] = login.Message;
                return View(loginViewModel);
            }

            return RedirectToAction("Index", "Todo");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            var logoutResponse = await _userRepository.LogoutAsync();

            return RedirectToAction("Login", "Account");
        }

    }
}
