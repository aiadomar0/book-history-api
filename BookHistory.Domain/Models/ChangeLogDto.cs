namespace BookHistory.Domain.Models;

public class ChangeLogDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string? BookTitle { get; set; }
    public DateTime ChangedAt { get; set; }
    public string ChangeType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}