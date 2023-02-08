using TodoApp_MVC.Models;

namespace TodoApp_MVC.Repositories.TodoRepo
{
    public interface ITodoRepository
    {
        Task SaveAsync();
        //public void DeleteAsync(int id);
        Task UpdateAsync(Todo todo);
    }
}
