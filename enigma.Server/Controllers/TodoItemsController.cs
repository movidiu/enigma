using Microsoft.AspNetCore.Mvc;
using enigma.Server.Models;
using enigma.Server.Services;

namespace enigma.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _service;

        public TodoItemsController(ITodoItemService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetTodoItems")]
        public async Task<IEnumerable<TodoItem>> Get()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}", Name = "GetTodoItemById")]
        public async Task<ActionResult<TodoItem>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost(Name = "CreateTodoItem")]
        public async Task<ActionResult<TodoItem>> Create(TodoItem item)
        {
            var created = await _service.CreateAsync(item);
            return CreatedAtRoute("GetTodoItemById", new { id = created.Id }, created);
        }

        [HttpPut("{id}", Name = "UpdateTodoItem")]
        public async Task<IActionResult> Update(int id, TodoItem item)
        {
            var updated = await _service.UpdateAsync(id, item);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteTodoItem")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
