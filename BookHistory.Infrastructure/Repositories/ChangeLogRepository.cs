namespace BookHistory.Infrastructure.Repositories;

using BookHistory.Domain.Entities;
using BookHistory.Domain.Interfaces;
using BookHistory.Domain.Models;
using BookHistory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class ChangeLogRepository : IChangeLogRepository
{
    private readonly BookDbContext _ctx;

    public ChangeLogRepository(BookDbContext ctx) => _ctx = ctx;

    public async Task AddRangeAsync(IEnumerable<BookChangeLog> logs)
    {
        await _ctx.ChangeLogs.AddRangeAsync(logs);
        await _ctx.SaveChangesAsync();
    }

    public async Task<PagedResult<BookChangeLog>> GetPagedAsync(ChangeLogQuery query)
    {
        IQueryable<BookChangeLog> q = _ctx.ChangeLogs.Include(cl => cl.Book);

        if (query.BookId.HasValue)
            q = q.Where(cl => cl.BookId == query.BookId.Value);

        if (!string.IsNullOrWhiteSpace(query.ChangeType))
            q = q.Where(cl => cl.ChangeType == query.ChangeType);
        if (query.From.HasValue)
            q = q.Where(cl => cl.ChangedAt >= query.From.Value);

        if (query.To.HasValue)
            q = q.Where(cl => cl.ChangedAt <= query.To.Value);

        q = (query.OrderBy?.ToLower(), query.Descending) switch
        {
            ("changedat", true)   => q.OrderByDescending(cl => cl.ChangedAt),
            ("changedat", false)  => q.OrderBy(cl => cl.ChangedAt),
            ("changetype", true)  => q.OrderByDescending(cl => cl.ChangeType),
            ("changetype", false) => q.OrderBy(cl => cl.ChangeType),
            ("booktitle", true)   => q.OrderByDescending(cl => cl.Book!.Title),
            ("booktitle", false)  => q.OrderBy(cl => cl.Book!.Title),
            _                     => q.OrderByDescending(cl => cl.ChangedAt)
        };

        var total = await q.CountAsync();
        var items = await q
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<BookChangeLog>
        {
            Items = items,
            TotalCount = total,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }
}