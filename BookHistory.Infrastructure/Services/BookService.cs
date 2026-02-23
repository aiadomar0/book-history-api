namespace BookHistory.Infrastructure.Services;

using BookHistory.Domain.Entities;
using BookHistory.Domain.Interfaces;
using BookHistory.Domain.Models;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepo;
    private readonly IChangeLogRepository _logRepo;

    public BookService(IBookRepository bookRepo, IChangeLogRepository logRepo)
    {
        _bookRepo = bookRepo;
        _logRepo = logRepo;
    }

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
    {
        var books = await _bookRepo.GetAllAsync();
        return books.Select(ToDto);
    }

    public async Task<BookDto?> GetBookByIdAsync(Guid id)
    {
        var book = await _bookRepo.GetByIdAsync(id);
        return book is null ? null : ToDto(book);
    }

    public async Task<BookDto> CreateBookAsync(CreateBookDto dto)
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            PublishDate = dto.PublishDate,
            Authors = dto.Authors
        };

        await _bookRepo.AddAsync(book);

        await _logRepo.AddRangeAsync(new[]
        {
            new BookChangeLog
            {
                Id = Guid.NewGuid(), BookId = book.Id,
                ChangedAt = DateTime.UtcNow,
                ChangeType = "BookCreated",
                Description = $"Book \"{book.Title}\" was created",
                NewValue = book.Title
            }
        });

        return ToDto(book);
    }

    public async Task<BookDto?> UpdateBookAsync(Guid id, UpdateBookDto dto)
    {
        var book = await _bookRepo.GetByIdAsync(id);
        if (book is null) return null;

        var logs = GenerateChangeLogs(book, dto).ToList();

        book.Title = dto.Title;
        book.Description = dto.Description;
        book.PublishDate = dto.PublishDate;
        book.Authors = dto.Authors;

        await _bookRepo.UpdateAsync(book);

        if (logs.Any())
            await _logRepo.AddRangeAsync(logs);

        return ToDto(book);
    }

    public async Task<bool> DeleteBookAsync(Guid id)
    {
        var book = await _bookRepo.GetByIdAsync(id);
        if (book is null) return false;

        await _logRepo.AddRangeAsync(new[]
        {
            new BookChangeLog
            {
                Id = Guid.NewGuid(), BookId = book.Id,
                ChangedAt = DateTime.UtcNow,
                ChangeType = "BookDeleted",
                Description = $"Book \"{book.Title}\" was deleted",
                OldValue = book.Title
            }
        });

        await _bookRepo.DeleteAsync(book);
        return true;
    }

    public async Task<PagedResult<ChangeLogDto>> GetChangeLogsAsync(ChangeLogQuery query)
    {
        var paged = await _logRepo.GetPagedAsync(query);
        return new PagedResult<ChangeLogDto>
        {
            Items = paged.Items.Select(ToLogDto),
            TotalCount = paged.TotalCount,
            Page = paged.Page,
            PageSize = paged.PageSize
        };
    }

    public async Task<IEnumerable<GroupedChangeLogResult>> GetGroupedChangeLogsAsync(ChangeLogQuery query)
    {
        var bigQuery = new ChangeLogQuery
        {
            Page = 1, PageSize = int.MaxValue,
            BookId = query.BookId, ChangeType = query.ChangeType,
            From = query.From, To = query.To,
            OrderBy = query.OrderBy, Descending = query.Descending
        };

        var paged = await _logRepo.GetPagedAsync(bigQuery);
        var logs = paged.Items.Select(ToLogDto).ToList();

        return query.GroupBy?.ToLower() switch
        {
            "book" => logs
                .GroupBy(l => l.BookTitle ?? l.BookId.ToString())
                .Select(g => new GroupedChangeLogResult { GroupKey = g.Key, Items = g }),

            "changetype" => logs
                .GroupBy(l => l.ChangeType)
                .Select(g => new GroupedChangeLogResult { GroupKey = g.Key, Items = g }),

            "date" => logs
                .GroupBy(l => l.ChangedAt.Date.ToString("yyyy-MM-dd"))
                .Select(g => new GroupedChangeLogResult { GroupKey = g.Key, Items = g }),

            _ => logs
                .GroupBy(l => l.BookTitle ?? l.BookId.ToString())
                .Select(g => new GroupedChangeLogResult { GroupKey = g.Key, Items = g })
        };
    }

    private IEnumerable<BookChangeLog> GenerateChangeLogs(Book old, UpdateBookDto updated)
    {
        var now = DateTime.UtcNow;

        if (old.Title != updated.Title)
            yield return new BookChangeLog
            {
                Id = Guid.NewGuid(), BookId = old.Id, ChangedAt = now,
                ChangeType = "TitleChanged",
                Description = $"Title was changed to \"{updated.Title}\"",
                OldValue = old.Title, NewValue = updated.Title
            };

        if (old.Description != updated.Description)
            yield return new BookChangeLog
            {
                Id = Guid.NewGuid(), BookId = old.Id, ChangedAt = now,
                ChangeType = "DescriptionChanged",
                Description = "Description was updated",
                OldValue = old.Description, NewValue = updated.Description
            };

        if (old.PublishDate != updated.PublishDate)
            yield return new BookChangeLog
            {
                Id = Guid.NewGuid(), BookId = old.Id, ChangedAt = now,
                ChangeType = "PublishDateChanged",
                Description = $"Publish date was changed to {updated.PublishDate:yyyy-MM-dd}",
                OldValue = old.PublishDate.ToString(), NewValue = updated.PublishDate.ToString()
            };

        foreach (var added in updated.Authors.Except(old.Authors))
            yield return new BookChangeLog
            {
                Id = Guid.NewGuid(), BookId = old.Id, ChangedAt = now,
                ChangeType = "AuthorAdded",
                Description = $"Author \"{added}\" was added",
                NewValue = added
            };

        foreach (var removed in old.Authors.Except(updated.Authors))
            yield return new BookChangeLog
            {
                Id = Guid.NewGuid(), BookId = old.Id, ChangedAt = now,
                ChangeType = "AuthorRemoved",
                Description = $"Author \"{removed}\" was removed",
                OldValue = removed
            };
    }

    private static BookDto ToDto(Book b) => new()
    {
        Id = b.Id, Title = b.Title, Description = b.Description,
        PublishDate = b.PublishDate, Authors = b.Authors
    };

    private static ChangeLogDto ToLogDto(BookChangeLog cl) => new()
    {
        Id = cl.Id, BookId = cl.BookId, BookTitle = cl.Book?.Title,
        ChangedAt = cl.ChangedAt, ChangeType = cl.ChangeType,
        Description = cl.Description, OldValue = cl.OldValue, NewValue = cl.NewValue
    };
}