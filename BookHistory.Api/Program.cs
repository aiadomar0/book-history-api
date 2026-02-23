using BookHistory.Api;
using BookHistory.Domain.Interfaces;
using BookHistory.Infrastructure.Persistence;
using BookHistory.Infrastructure.Repositories;
using BookHistory.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookDbContext>(opt =>
    opt.UseSqlite("Data Source=bookhistory.db"));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IChangeLogRepository, ChangeLogRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Book History API", Version = "v1" });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookDbContext>();
    db.Database.Migrate();
    await SeedData.InitializeAsync(db);
}

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book History API v1");
    c.RoutePrefix = string.Empty;
});

app.UseAuthorization();
app.MapControllers();

app.Run();