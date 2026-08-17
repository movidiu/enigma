using Microsoft.EntityFrameworkCore;
using enigma.Server.Data;
using enigma.Server.Models;

namespace enigma.Server.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly EnigmaDbContext _context;

        public TodoItemRepository(EnigmaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<TodoItem> AddAsync(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(TodoItem item)
        {
            _context.TodoItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item is null)
            {
                return;
            }

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
