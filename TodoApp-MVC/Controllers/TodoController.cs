using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApp_MVC.Data;
using TodoApp_MVC.Models;
using TodoApp_MVC.Repositories.TodoRepo;
using TodoApp_MVC.ViewModels.Todo;

namespace TodoApp_MVC.Controllers
{
    public class TodoController : Controller
    {
        private readonly ApplicationDataContext _context;
        //private readonly ITodoRepository _repo;
        public TodoController(ApplicationDataContext context)
        {
            _context = context;
            //_repo = repo;
        }
        // GET: TodoController
        public async Task<ActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity?)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var todoModel = await _context.Todos.Where(i => i.AppUserId == claim.Value).ToListAsync();
            
            return View(todoModel);
        }

        // GET: TodoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var todoDetails = await _context.Todos.FirstOrDefaultAsync(u => u.Id == id);
            
            if(todoDetails != null)
            {
                var todoResponse = new UpdateTodoViewModel
                {
                    Id = todoDetails.Id,
                    Title = todoDetails.Title,
                    Description = todoDetails.Description,
                    IsComplete = todoDetails.IsComplete
                };

                return View(todoResponse);
            }

            TempData["Error"] = "Todo cannot be found";

            return NotFound();
        }

        // GET: TodoController/Create
        public ActionResult Create()
        {
            var createTodo = new CreateTodoViewModel();
            
            return View(createTodo);
        }

        // POST: TodoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateTodoViewModel todoViewModel)
        {
            if (!ModelState.IsValid) return View();

            var claimsIdentity = (ClaimsIdentity?)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var todos = new Todo()
            {
                Title = todoViewModel.Title,
                Description = todoViewModel.Description,
                IsComplete = todoViewModel.IsComplete,
                AppUserId = claim.Value
            };

            await _context.Todos.AddAsync(todos);
            await _context.SaveChangesAsync();



            return RedirectToAction("Index");
        }

        // GET: TodoController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);
            var response = new UpdateTodoViewModel(){
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsComplete = todo.IsComplete
            };
            return View(response);
        }

        // POST: TodoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateTodoViewModel todoViewModel)
        {
            // Todo: Validate if Valid

            var claimsIdentity = (ClaimsIdentity?)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);



            if (ModelState.IsValid)
            {

                var todoVM = new Todo
                {
                    Id = todoViewModel.Id,
                    Title = todoViewModel.Title,
                    Description = todoViewModel.Description,
                    AppUserId = claim.Value
                };


                _context.Todos.Update(todoVM);
                 _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return NotFound();
        }

        [HttpGet, ActionName("Delete")]
        public async Task<ActionResult> DeleteDetail(int id)
        {
            var todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);


            var response = new DeleteTodoViewModel { TodoId = id };
            
            return View("DeleteDetail", response);
        }


        // POST: TodoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var todo = await _context.Todos.FirstOrDefaultAsync(i => i.Id == id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}
