namespace BookHistory.Infrastructure.Repositories;

using BookHistory.Domain.Entities;
using BookHistory.Domain.Interfaces;
using BookHistory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class BookRepository : IBookRepository
{
    private readonly BookDbContext _ctx;

    public BookRepository(BookDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Book>> GetAllAsync() =>
        await _ctx.Books.ToListAsync();

    public async Task<Book?> GetByIdAsync(Guid id) =>
        await _ctx.Books.FirstOrDefaultAsync(b => b.Id == id);

    public async Task AddAsync(Book book)
    {
        await _ctx.Books.AddAsync(book);
        await _ctx.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        _ctx.Books.Update(book);
        await _ctx.SaveChangesAsync();
    }
}