using Microsoft.EntityFrameworkCore;
using enigma.Server.Models;

namespace enigma.Server.Data
{
    public class EnigmaDbContext : DbContext
    {
        public EnigmaDbContext(DbContextOptions<EnigmaDbContext> options) : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    }
}
