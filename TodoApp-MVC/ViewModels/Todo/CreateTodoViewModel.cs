using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TodoApp_MVC.ViewModels.Todo
{
    public class CreateTodoViewModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsComplete { get; set; } = false;
        [ValidateNever]
        public string AppUserId { get; set; }
    }
}
