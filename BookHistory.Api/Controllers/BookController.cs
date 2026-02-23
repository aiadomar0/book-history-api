namespace BookHistory.Api.Controllers;

using BookHistory.Domain.Interfaces;
using BookHistory.Domain.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;

    public BooksController(IBookService service) => _service = service;

    /// <summary>Returns all books</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _service.GetAllBooksAsync();
        return Ok(books);
    }

    /// <summary>Returns a single book by id</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var book = await _service.GetBookByIdAsync(id);
        return book is null ? NotFound() : Ok(book);
    }

    /// <summary>Creates a new book</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookDto dto)
    {
        var created = await _service.CreateBookAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Updates a book and records changelog entries for every changed field</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBookDto dto)
    {
        var updated = await _service.UpdateBookAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Deletes a book and records a BookDeleted changelog entry</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteBookAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}