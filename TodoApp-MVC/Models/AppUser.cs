using Microsoft.AspNetCore.Identity;

namespace TodoApp_MVC.Models
{
    public class AppUser: IdentityUser
    {
        //public string FirstName { get; set; }
        //public string LastName { get; set; }

        
        
        // Navigation Properties
        public virtual ICollection<Todo> Todos { get; set; }
    }
}
