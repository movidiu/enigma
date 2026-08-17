using enigma.Server.Models;

namespace enigma.Server.Repositories
{
    public interface ITodoItemRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();

        Task<TodoItem?> GetByIdAsync(int id);

        Task<TodoItem> AddAsync(TodoItem item);

        Task UpdateAsync(TodoItem item);

        Task DeleteAsync(int id);
    }
}
