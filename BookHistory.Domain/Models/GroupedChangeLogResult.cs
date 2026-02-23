namespace BookHistory.Domain.Models;

public class GroupedChangeLogResult
{
    public string GroupKey { get; set; } = string.Empty;
    public IEnumerable<ChangeLogDto> Items { get; set; } = Enumerable.Empty<ChangeLogDto>();
}