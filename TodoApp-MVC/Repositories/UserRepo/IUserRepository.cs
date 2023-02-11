using TodoApp_MVC.Models;
using TodoApp_MVC.ViewModels.Account;

namespace TodoApp_MVC.Repositories.UserRepo
{
    public interface IUserRepository
    {
      Task<Status> RegisterAsync(RegisterViewModel registerVM);
      Task<AppUser> FindUserByEmailAsync(string emailAddress);
      Task<Status> LoginAsync(LoginViewModel model);
      Task<Status> LogoutAsync();
    }
}
