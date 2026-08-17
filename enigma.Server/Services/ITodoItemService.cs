using enigma.Server.Models;

namespace enigma.Server.Services
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();

        Task<TodoItem?> GetByIdAsync(int id);

        Task<TodoItem> CreateAsync(TodoItem item);

        Task<bool> UpdateAsync(int id, TodoItem item);

        Task<bool> DeleteAsync(int id);
    }
}
