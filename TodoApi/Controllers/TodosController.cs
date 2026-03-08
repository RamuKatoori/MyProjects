using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly TodoDbContext _db;
        public TodosController(TodoDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            var list = await _db.Todos.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            var todo = await _db.Todos.FirstOrDefaultAsync(t => t.Id == id);
            if (todo is null) return NotFound();
            return Ok(todo);
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo([FromBody] Todo todo)
        {
            _db.Todos.Add(todo);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] Todo input)
        {
            if (id != input.Id) return BadRequest("Id mismatch");
            var todo = await _db.Todos.FirstOrDefaultAsync(t => t.Id == id);
            if (todo is null) return NotFound();

            todo.Title = input.Title;
            todo.IsComplete = input.IsComplete;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _db.Todos.FirstOrDefaultAsync(t => t.Id == id);
            if (todo is null) return NotFound();

            _db.Todos.Remove(todo);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
