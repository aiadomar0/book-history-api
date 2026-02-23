namespace BookHistory.Domain.Interfaces;

using BookHistory.Domain.Entities;
using BookHistory.Domain.Models;

public interface IChangeLogRepository
{
    Task AddRangeAsync(IEnumerable<BookChangeLog> logs);
    Task<PagedResult<BookChangeLog>> GetPagedAsync(ChangeLogQuery query);
}