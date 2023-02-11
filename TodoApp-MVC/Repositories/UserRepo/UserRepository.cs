using Microsoft.AspNetCore.Identity;
using TodoApp_MVC.Data;
using TodoApp_MVC.Models;
using TodoApp_MVC.ViewModels.Account;

namespace TodoApp_MVC.Repositories.UserRepo
{
    public class UserRepository: IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDataContext _context;

        public UserRepository(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ApplicationDataContext context) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<Status> LoginAsync(LoginViewModel model)
        {
            var message = new Status();

            var user = await FindUserByEmailAsync(model.EmailAddress);
           
            if (user == null)
            {
                message = new Status
                {
                    Message = $"User {model.EmailAddress} not exist",
                    IsSuccess = false
                };

                return message;
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordCheck)
            {
                message = new Status
                {
                    Message = "Password Failed",
                    IsSuccess = false
                };

                return message;
            }

            var signInUser = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!signInUser.Succeeded)
            {
                message = new Status
                {
                    Message = "Sign In Failed",
                    IsSuccess = true
                };

                return message;
            }

            return message;
        }

        public async Task<AppUser> FindUserByEmailAsync(string emailAddress)
        {
            var user = await _userManager.FindByEmailAsync(emailAddress);
            
            return user;
        }
        public async Task<Status> RegisterAsync(RegisterViewModel registerVM)
        {
            var message = new Status();

            var userExist = await FindUserByEmailAsync(registerVM.EmailAddress);
            
            if (userExist != null)
            {
                message = new Status
                {
                    Message = $"User email {registerVM.EmailAddress} was already exist",
                    IsSuccess = false
                };

                return message;
            }

            var newUser = new AppUser
            {
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!newUserResponse.Succeeded)
            {
                message = new Status
                
                {
                    Message = $"Create User {newUser.UserName} failed",
                    IsSuccess = false
                };
                
                return message;
            } 


           var addUserToRoles = await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            if (!addUserToRoles.Succeeded)
            {
                message = new Status
                {
                    Message = $"Add user {newUser.UserName} to role failed!",
                    IsSuccess = false
                };

                return message;
            }

            return message;
        }

        public async Task<Status> LogoutAsync()
        {
            var message = new Status();

            await  _signInManager.SignOutAsync();

            return message;
        }
    }
}
