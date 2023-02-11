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

        private bool save()
        {
            var isSaved = _context.SaveChanges();
            
            return isSaved > 0;
        }
        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            var todos = await _context.Todos.ToListAsync();
            
            return todos;
        }

        public async Task<IEnumerable<Todo>> GetAllByUser(string user)
        {
            var todos =  await GetAllAsync();
            var filterByUser = todos.Where(x => x.AppUserId == user);

            return filterByUser;
        }

        public async Task<bool> AddAsync(Todo todo)
        {
            await _context.Todos.AddAsync(todo);

            return save();
        }

        public async Task<Todo?> GetByIdAsync(int id)
        {
            var todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            return todo;
        }

        public async Task<bool> Update(Todo todo)
        {
           _context.Update(todo);
            
            return save();
        }

        public async Task<Todo> GetAsNoTracking(int id)
        {
            return await _context.Todos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id); 
        }

        public bool DeleteAsync(Todo todo)
        {
            _context.Todos.Remove(todo);

            return save();
        }



        //Task<IEnumerable<Todo>> GetAllAsync();
        //Task<Todo?> GetByIdAsync(int id);
        //Task AddAsync(Todo todo);
        ////public void DeleteAsync(int id);
        //Task UpdateAsync(Todo todo);

    }
}
