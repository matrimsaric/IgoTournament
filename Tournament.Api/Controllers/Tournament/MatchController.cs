using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Tournament.Api.Controllers.Tournament
{
        [ApiController]
        [Route("api/[controller]")]
        public class MatchController : ControllerBase
        {
            private readonly IMatchRepository _matchRepository;

            public MatchController(IMatchRepository matchRepository)
            {
                _matchRepository = matchRepository;
            }

            // GET: api/match
            [HttpGet]
            public async Task<IActionResult> GetAllMatches([FromQuery] bool reload = true)
            {
                var matches = await _matchRepository.GetAllMatches(reload);
                return Ok(matches);
            }

            // GET: api/match/{id}
            [HttpGet("{id:guid}")]
            public async Task<IActionResult> GetMatchById(Guid id, [FromQuery] bool reload = true)
            {
                var match = await _matchRepository.GetMatchById(id, reload);

                if (match == null)
                    return NotFound($"Match with id {id} not found.");

                return Ok(match);
            }

            // POST: api/match
            [HttpPost]
            public async Task<IActionResult> CreateMatch([FromBody] Match newMatch, [FromQuery] bool reload = true)
            {
                if (newMatch == null)
                    return BadRequest("Match cannot be null.");

                var result = await _matchRepository.CreateMatch(newMatch, reload);

                return Ok(result);
            }

            // PUT: api/match
            [HttpPut]
            public async Task<IActionResult> UpdateMatch([FromBody] Match updatedMatch, [FromQuery] bool reload = true)
            {
                if (updatedMatch == null)
                    return BadRequest("Match cannot be null.");

                var result = await _matchRepository.UpdateMatch(updatedMatch, reload);

                return Ok(result);
            }

            // DELETE: api/match/{id}
            [HttpDelete("{id:guid}")]
            public async Task<IActionResult> DeleteMatch(Guid id, [FromQuery] bool reload = true)
            {
                var existing = await _matchRepository.GetMatchById(id, reload);

                if (existing == null)
                    return NotFound($"Match with id {id} not found.");

                var result = await _matchRepository.DeleteMatch(existing, reload);

                return Ok(result);
            }
        }
    }
