using Microsoft.AspNetCore.Mvc;
using CompetitionDomain.Model;
using CompetitionDomain.Services.Interfaces;

namespace Tournament.Api.Controllers.Tournament
{
    [ApiController]
    [Route("api/content/matches")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _service;

        public MatchController(IMatchService service)
        {
            _service = service;
        }

        // GET: api/content/matches
        [HttpGet]
        public async Task<IActionResult> GetAllMatches()
        {
            var matches = await _service.GetAllMatchesAsync();
            return Ok(matches);
        }

        // GET: api/content/matches/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMatch(Guid id)
        {
            var match = await _service.GetMatchByIdAsync(id);
            return match is null ? NotFound() : Ok(match);
        }

        // POST: api/content/matches
        [HttpPost]
        public async Task<IActionResult> CreateMatch([FromBody] Match newMatch)
        {
            var result = await _service.CreateMatchAsync(newMatch);
            return Ok(result);
        }

        // PUT: api/content/matches/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateMatch(Guid id, [FromBody] Match updatedMatch)
        {
            updatedMatch.Id = id;
            var result = await _service.UpdateMatchAsync(updatedMatch);
            return Ok(result);
        }

        // DELETE: api/content/matches/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteMatch(Guid id)
        {
            var result = await _service.DeleteMatchAsync(id);
            return Ok(result);
        }
    }
}
