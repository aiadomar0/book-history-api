namespace BookHistory.Infrastructure.Persistence;

using BookHistory.Domain.Entities;

public static class SeedData
{
    public static async Task InitializeAsync(BookDbContext ctx)
    {
        if (ctx.Books.Any()) return;

        var book1 = new Book
        {
            Id = Guid.NewGuid(),
            Title = "The Hobbit",
            Description = "A fantasy novel by J.R.R. Tolkien",
            PublishDate = new DateOnly(1937, 9, 21),
            Authors = new List<string> { "J.R.R. Tolkien" }
        };

        var book2 = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Clean Code",
            Description = "A handbook of agile software craftsmanship",
            PublishDate = new DateOnly(2008, 8, 1),
            Authors = new List<string> { "Robert C. Martin" }
        };

        await ctx.Books.AddRangeAsync(book1, book2);

        var now = DateTime.UtcNow;

        await ctx.ChangeLogs.AddRangeAsync(
            new BookChangeLog
            {
                Id = Guid.NewGuid(), BookId = book1.Id,
                ChangedAt = now.AddDays(-10), ChangeType = "BookCreated",
                Description = "Book \"The Hobbit\" was created",
                NewValue = "The Hobbit"
            },
            new BookChangeLog
            {
                Id = Guid.NewGuid(), BookId = book1.Id,
                ChangedAt = now.AddDays(-5), ChangeType = "DescriptionChanged",
                Description = "Description was updated",
                OldValue = "A short fantasy tale",
                NewValue = "A fantasy novel by J.R.R. Tolkien"
            },
            new BookChangeLog
            {
                Id = Guid.NewGuid(), BookId = book2.Id,
                ChangedAt = now.AddDays(-8), ChangeType = "BookCreated",
                Description = "Book \"Clean Code\" was created",
                NewValue = "Clean Code"
            },
            new BookChangeLog
            {
                Id = Guid.NewGuid(), BookId = book2.Id,
                ChangedAt = now.AddDays(-2), ChangeType = "AuthorAdded",
                Description = "Author \"Dean Wampler\" was added",
                NewValue = "Dean Wampler"
            }
        );

        await ctx.SaveChangesAsync();
    }
}