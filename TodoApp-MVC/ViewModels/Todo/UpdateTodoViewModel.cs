namespace TodoApp_MVC.ViewModels.Todo
{
    public class UpdateTodoViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsComplete { get; set; } = false;
        public string? UserId { get; set; }
    }
}
