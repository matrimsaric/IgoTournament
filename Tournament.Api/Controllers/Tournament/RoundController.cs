using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Tournament.Api.Controllers.Tournament
{
        [ApiController]
        [Route("api/[controller]")]
        public class RoundController : ControllerBase
        {
            private readonly IRoundRepository _roundRepository;

            public RoundController(IRoundRepository roundRepository)
            {
                _roundRepository = roundRepository;
            }

            // GET: api/round
            [HttpGet]
            public async Task<IActionResult> GetAllRounds([FromQuery] bool reload = true)
            {
                var rounds = await _roundRepository.GetAllRounds(reload);
                return Ok(rounds);
            }

            // GET: api/round/{id}
            [HttpGet("{id:guid}")]
            public async Task<IActionResult> GetRoundById(Guid id, [FromQuery] bool reload = true)
            {
                var round = await _roundRepository.GetRoundById(id, reload);

                if (round == null)
                    return NotFound($"Round with id {id} not found.");

                return Ok(round);
            }

            // POST: api/round
            [HttpPost]
            public async Task<IActionResult> CreateRound([FromBody] Round newRound, [FromQuery] bool reload = true)
            {
                if (newRound == null)
                    return BadRequest("Round cannot be null.");

                var result = await _roundRepository.CreateRound(newRound, reload);

                return Ok(result);
            }

            // PUT: api/round
            [HttpPut]
            public async Task<IActionResult> UpdateRound([FromBody] Round updatedRound, [FromQuery] bool reload = true)
            {
                if (updatedRound == null)
                    return BadRequest("Round cannot be null.");

                var result = await _roundRepository.UpdateRound(updatedRound, reload);

                return Ok(result);
            }

            // DELETE: api/round/{id}
            [HttpDelete("{id:guid}")]
            public async Task<IActionResult> DeleteRound(Guid id, [FromQuery] bool reload = true)
            {
                var existing = await _roundRepository.GetRoundById(id, reload);

                if (existing == null)
                    return NotFound($"Round with id {id} not found.");

                var result = await _roundRepository.DeleteRound(existing, reload);

                return Ok(result);
            }
        }
    }
