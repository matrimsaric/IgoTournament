using JosekiDomain.Model;
using JosekiDomain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/content/joseki")]
public class JosekiController : ControllerBase
{
    private readonly IJosekiEntryService _entryService;

    public JosekiController(IJosekiEntryService entryService)
    {
        _entryService = entryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllJoseki()
    {
        var entries = await _entryService.GetAllJosekiAsync();
        return Ok(entries);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetJoseki(Guid id)
    {
        var entry = await _entryService.GetJosekiByIdAsync(id);
        return entry is null ? NotFound() : Ok(entry);
    }

    [HttpGet("category/{category:int}")]
    public async Task<IActionResult> GetByCategory(int category)
    {
        var entries = await _entryService.GetByCategoryAsync(category);
        return Ok(entries);
    }

    [HttpGet("{id:guid}/children")]
    public async Task<IActionResult> GetChildren(Guid id)
    {
        var children = await _entryService.GetChildrenAsync(id);
        return Ok(children);
    }

    [HttpGet("roots")]
    public async Task<IActionResult> GetRootJoseki()
    {
        var roots = await _entryService.GetRootJosekiAsync();
        return Ok(roots);
    }

    [HttpGet("book/{bookId:guid}")]
    public async Task<IActionResult> GetByBook(Guid bookId)
    {
        var entries = await _entryService.GetByBookAsync(bookId);
        return Ok(entries);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJoseki([FromBody] JosekiEntry newEntry)
    {
        var result = await _entryService.CreateJosekiAsync(newEntry);
        return Ok(newEntry);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateJoseki(Guid id, [FromBody] JosekiEntry updatedEntry)
    {
        updatedEntry.Id = id;
        var result = await _entryService.UpdateJosekiAsync(updatedEntry);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteJoseki(Guid id)
    {
        var result = await _entryService.DeleteJosekiAsync(id);
        return Ok(result);
    }
}
