namespace BookHistory.Domain.Entities;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly PublishDate { get; set; }
    public List<string> Authors { get; set; } = new();
    public ICollection<BookChangeLog> ChangeLogs { get; set; } = new List<BookChangeLog>();
}