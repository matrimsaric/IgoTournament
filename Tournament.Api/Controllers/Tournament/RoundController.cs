using CompetitionDomain.Model;
using CompetitionDomain.Services;
using CompetitionDomain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/content/rounds")]
public class RoundController : ControllerBase
{
    private readonly IRoundService _service;
    private readonly IMatchService _matchService;

    public RoundController(IRoundService service, IMatchService matchService    )
    {
        _service = service;
        _matchService = matchService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRounds()
    {
        var rounds = await _service.GetAllRoundsAsync();
        return Ok(rounds);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRound(Guid id)
    {
        var round = await _service.GetRoundByIdAsync(id);
        return round is null ? NotFound() : Ok(round);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRound([FromBody] Round newRound)
    {
        var result = await _service.CreateRoundAsync(newRound);
        return Ok(newRound);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRound(Guid id, [FromBody] Round updatedRound)
    {
        updatedRound.Id = id;
        var result = await _service.UpdateRoundAsync(updatedRound);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRound(Guid id)
    {
        var result = await _service.DeleteRoundAsync(id);
        return Ok(result);
    }

    // GET: api/content/rounds/{id}/matches
    [HttpGet("{id:guid}/matches")]
    public async Task<IActionResult> GetMatchesForRound(Guid id)
    {
        // Get all matches
        var matches = await _matchService.GetAllMatchesAsync();

        // Filter by round ID
        var filtered = matches
            .Where(m => m.RoundId == id)
            .ToList();

        return Ok(filtered);
    }

}
