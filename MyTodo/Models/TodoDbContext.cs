using Microsoft.EntityFrameworkCore;

namespace MyTodo.Models
{
    public class TodoDbContext: DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options): base(options)
        {

        }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ForSqlServerUseIdentityColumns().Entity<Todo>().HasIndex(t => t.Id).IsUnique();
        }
    }
}
