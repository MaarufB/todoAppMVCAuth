using Microsoft.EntityFrameworkCore;
using TodoApp_MVC.Data;
using TodoApp_MVC.Models;

namespace TodoApp_MVC.Repositories.TodoRepo
{
    public class TodoRepository: ITodoRepository
    {
        private readonly ApplicationDataContext _context;

        public TodoRepository(ApplicationDataContext context)
        {
            _context = context;
        }

        public  async Task  SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Todo todo)
        {
            _context.Entry<Todo>(todo).State = EntityState.Modified;
        }


    }
}
