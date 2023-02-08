using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp_MVC.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsComplete { get; set; }

        // Navigation Props/FK for User who owns the todo list


        public string? AppUserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public AppUser? AppUser { get; set; }
    }
}
