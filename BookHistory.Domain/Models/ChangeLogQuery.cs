namespace BookHistory.Domain.Models;

public class ChangeLogQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public Guid? BookId { get; set; }
    public string? ChangeType { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string OrderBy { get; set; } = "ChangedAt";
    public bool Descending { get; set; } = true;
    public string? GroupBy { get; set; }
}