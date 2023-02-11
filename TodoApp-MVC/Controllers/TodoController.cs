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
        private readonly ITodoRepository _todoRepository;
        public TodoController(ApplicationDataContext context, ITodoRepository todoRepository)
        {
            _context = context;
            _todoRepository= todoRepository;
        }
        // GET: TodoController
        public async Task<ActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity?)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            var todos = await _todoRepository.GetAllByUser(claim.Value);
            ViewData["Username"] = User.Identity.Name;

            return View(todos);
        }

        // GET: TodoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var todoDetails = await _context.Todos.FirstOrDefaultAsync(u => u.Id == id);
            
            if(todoDetails == null)
            {
                TempData["Error"] = "Todo cannot be found";

                return NotFound();
            }

            var todoResponse = new UpdateTodoViewModel
            {
                Id = todoDetails.Id,
                Title = todoDetails.Title,
                Description = todoDetails.Description,
                IsComplete = todoDetails.IsComplete
            };

            return View(todoResponse);


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

            var todo = new Todo()
            {
                Title = todoViewModel.Title,
                Description = todoViewModel.Description,
                IsComplete = todoViewModel.IsComplete,
                AppUserId = claim.Value
            };

           await _todoRepository.AddAsync(todo);

            return RedirectToAction("Index");
        }

        // GET: TodoController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var todo = await _todoRepository.GetAsNoTracking(id);
            
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
        public async Task<ActionResult> Edit(UpdateTodoViewModel todoViewModel)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Fields");
                return View(todoViewModel);
            }

            var claimsIdentity = (ClaimsIdentity?)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            var todoVM = new Todo
            {
                Id = todoViewModel.Id,
                Title = todoViewModel.Title,
                Description = todoViewModel.Description,
                AppUserId = claim.Value
            };

            await _todoRepository.Update(todoVM);
          
            
            return RedirectToAction("Index");

        }

        [HttpGet, ActionName("Delete")]
        public async Task<ActionResult> DeleteDetail(int id)
        {
            var todo = await _todoRepository.GetAsNoTracking(id);

            var response = new DeleteTodoViewModel 
            { 
                Id = todo.Id, 
                Title = todo.Title, 
                Description= todo.Description, 
                IsComplete= todo.IsComplete
            };
            
            return View("DeleteDetail", response);
        }


        // POST: TodoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(DeleteTodoViewModel todoDeleteVM)
        {
            var todo = await _todoRepository.GetAsNoTracking(todoDeleteVM.Id);

            if (todo != null)
            {
                _todoRepository.DeleteAsync(todo);

                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}
