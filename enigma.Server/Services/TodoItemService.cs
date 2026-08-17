using enigma.Server.Models;
using enigma.Server.Repositories;

namespace enigma.Server.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _repository;

        public TodoItemService(ITodoItemRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<TodoItem?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<TodoItem> CreateAsync(TodoItem item)
        {
            item.CreatedAt = DateTime.UtcNow;
            return _repository.AddAsync(item);
        }

        public async Task<bool> UpdateAsync(int id, TodoItem item)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
            {
                return false;
            }

            existing.Title = item.Title;
            existing.IsDone = item.IsDone;

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
            {
                return false;
            }

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
