using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyTodo.Models;

namespace MyTodo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoDbContext dbContext;

        public TodosController(TodoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Todo>> Get()
        {
            return dbContext.Todos.OrderByDescending(t => t.Timestamp).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Todo> Get(int id)
        {
            var todo = dbContext.Todos.FirstOrDefault(t => t.Id == id);
            if (todo == null) return StatusCode((int)HttpStatusCode.NotFound);
            return todo;
        }

        // POST api/values
        [HttpPost]
        public ActionResult<Todo> Post([FromBody] Todo todo)
        {
            if (todo == null) return StatusCode((int)HttpStatusCode.NotAcceptable);
            todo.Timestamp = DateTime.Now;
            dbContext.Todos.Add(todo);
            dbContext.SaveChanges();
            return todo;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Todo todo)
        {
            var _todo = dbContext.Todos.FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                _todo.Completed = todo.Completed;
                dbContext.SaveChanges();
            }
        }

        [HttpPatch]
        public void Patch([FromBody] IEnumerable<Todo> todos)
        {
            if (todos != null)
            {
                dbContext.Todos.RemoveRange(dbContext.Todos.Where(t => todos.Any(s => t.Id == s.Id)));
                dbContext.SaveChanges();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var todo = dbContext.Todos.FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                dbContext.Todos.Remove(todo);
                dbContext.SaveChanges();
            }
        }
    }
}
