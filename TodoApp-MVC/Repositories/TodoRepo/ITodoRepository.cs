using TodoApp_MVC.Models;

namespace TodoApp_MVC.Repositories.TodoRepo
{
    public interface ITodoRepository
    {

        Task<IEnumerable<Todo>> GetAllAsync();
        Task<IEnumerable<Todo>> GetAllByUser(string userId);
        Task<Todo> GetAsNoTracking(int id);
        Task<Todo?> GetByIdAsync(int id);
        Task<bool> AddAsync(Todo todo);
        Task<bool> Update(Todo todo);
        bool DeleteAsync(Todo todo);
    }
}
