using JosekiDomain.Model;
using JosekiDomain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/content/joseki/books")]
public class JosekiBooksController : ControllerBase
{
    private readonly IJosekiBookService _bookService;

    public JosekiBooksController(IJosekiBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBook(Guid id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        return book is null ? NotFound() : Ok(book);
    }

    [HttpGet("search/{title}")]
    public async Task<IActionResult> SearchBooks(string title)
    {
        var books = await _bookService.SearchByTitleAsync(title);
        return Ok(books);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] JosekiBook newBook)
    {
        var result = await _bookService.CreateBookAsync(newBook);
        return Ok(newBook);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBook(Guid id, [FromBody] JosekiBook updatedBook)
    {
        updatedBook.Id = id;
        var result = await _bookService.UpdateBookAsync(updatedBook);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        var result = await _bookService.DeleteBookAsync(id);
        return Ok(result);
    }
}
