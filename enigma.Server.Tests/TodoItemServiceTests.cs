using Moq;
using enigma.Server.Models;
using enigma.Server.Repositories;
using enigma.Server.Services;
using Xunit;

namespace enigma.Server.Tests
{
    public class TodoItemServiceTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsItemsFromRepository()
        {
            var expected = new List<TodoItem>
            {
                new() { Id = 1, Title = "First", IsDone = false, CreatedAt = DateTime.UtcNow },
                new() { Id = 2, Title = "Second", IsDone = true, CreatedAt = DateTime.UtcNow }
            };

            var repositoryMock = new Mock<ITodoItemRepository>();
            repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(expected);

            var service = new TodoItemService(repositoryMock.Object);

            var result = await service.GetAllAsync();

            Assert.Equal(expected, result);
            repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenNotFound_ReturnsNull()
        {
            var repositoryMock = new Mock<ITodoItemRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TodoItem?)null);

            var service = new TodoItemService(repositoryMock.Object);

            var result = await service.GetByIdAsync(42);

            Assert.Null(result);
        }
    }
}
