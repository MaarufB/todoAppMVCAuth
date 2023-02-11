using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TodoApp_MVC.ViewModels.Todo
{
    public class DeleteTodoViewModel
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public bool IsComplete { get; set; } = false;
        [ValidateNever]
        public string AppUserId { get; set; }
    }
}
