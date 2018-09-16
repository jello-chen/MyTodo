using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTodo.Models
{
    public class TodoDbContextFactory : IDesignTimeDbContextFactory<TodoDbContext>
    {
        public TodoDbContext CreateDbContext(string[] args)
        {
            var dbBuilder = new DbContextOptionsBuilder<TodoDbContext>();
            dbBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = MyTodo; Trusted_Connection = True; MultipleActiveResultSets = true");
            return new TodoDbContext(dbBuilder.Options);
        }
    }
}
