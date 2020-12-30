using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItemDTO> TodoItems { get; set; }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<TodoApi.Models.Product> Product { get; set; }
    }
}
