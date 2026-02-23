namespace BookHistory.Infrastructure.Persistence;

using BookHistory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class BookDbContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<BookChangeLog> ChangeLogs => Set<BookChangeLog>();

    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .Property(b => b.Authors)
            .HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
            );

        modelBuilder.Entity<BookChangeLog>()
            .HasOne(cl => cl.Book)
            .WithMany(b => b.ChangeLogs)
            .HasForeignKey(cl => cl.BookId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<BookChangeLog>()
            .Property(cl => cl.BookId)
            .IsRequired(false);
    }
}