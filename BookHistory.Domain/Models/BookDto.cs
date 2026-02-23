namespace BookHistory.Domain.Models;

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly PublishDate { get; set; }
    public List<string> Authors { get; set; } = new();
}