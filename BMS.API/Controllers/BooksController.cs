using BMS.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BMS.Infrastructure.Data.BMSDBContext _dbContext;

    public BooksController(BMS.Infrastructure.Data.BMSDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAll(CancellationToken cancellationToken)
    {
        var books = await _dbContext.Books
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .ToListAsync(cancellationToken);
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Book>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var book = await _dbContext.Books.FindAsync([id], cancellationToken);
        if (book is null) return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> Create([FromBody] Book book, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(book.Title))
        {
            return BadRequest("Title is required.");
        }

        book.Id = Guid.NewGuid();
        book.CreatedUtc = DateTime.UtcNow;
        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Book input, CancellationToken cancellationToken)
    {
        var book = await _dbContext.Books.FindAsync([id], cancellationToken);
        if (book is null) return NotFound();

        book.Title = input.Title;
        book.Author = input.Author;
        book.Isbn = input.Isbn;
        book.PublishedYear = input.PublishedYear;
        book.UpdatedUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var book = await _dbContext.Books.FindAsync([id], cancellationToken);
        if (book is null) return NotFound();

        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}


