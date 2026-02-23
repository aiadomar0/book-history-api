namespace BookHistory.Domain.Interfaces;

using BookHistory.Domain.Models;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllBooksAsync();
    Task<BookDto?> GetBookByIdAsync(Guid id);
    Task<BookDto> CreateBookAsync(CreateBookDto dto);
    Task<BookDto?> UpdateBookAsync(Guid id, UpdateBookDto dto);
    Task<bool> DeleteBookAsync(Guid id);
    Task<PagedResult<ChangeLogDto>> GetChangeLogsAsync(ChangeLogQuery query);
    Task<IEnumerable<GroupedChangeLogResult>> GetGroupedChangeLogsAsync(ChangeLogQuery query);
}