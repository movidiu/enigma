namespace enigma.Server.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
