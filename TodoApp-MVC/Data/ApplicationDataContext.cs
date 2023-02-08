using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using TodoApp_MVC.Models;

namespace TodoApp_MVC.Data
{
    public class ApplicationDataContext: IdentityDbContext
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options): base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }
    }
}
