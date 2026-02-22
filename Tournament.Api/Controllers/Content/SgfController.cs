using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.ControlModule.Model;
using CompetitionDomain.ControlModule.Services;
using CompetitionDomain.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.ServerSentEvents;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tournament.Api.Controllers.Content
{
        [ApiController]
        [Route("api/[controller]")]
        public class SgfRecordController : ControllerBase
        {
            private readonly ISgfRecordRepository _sgfRepository;
        private readonly ISgfParser _sgfParser;

        public SgfRecordController(ISgfRecordRepository sgfRepository, ISgfParser sgfParser)
            {
                _sgfRepository = sgfRepository;
            _sgfParser = sgfParser;
        }

            // GET: api/sgfrecord
            [HttpGet]
            public async Task<IActionResult> GetAllSgfRecords([FromQuery] bool reload = true)
            {
                var records = await _sgfRepository.GetAllSgfRecords(reload);
                return Ok(records);
            }

            // GET: api/sgfrecord/{id}
            [HttpGet("{id:guid}")]
            public async Task<IActionResult> GetSgfRecordById(Guid id, [FromQuery] bool reload = true)
            {
                var record = await _sgfRepository.GetSgfRecordById(id, reload);

                if (record == null)
                    return NotFound($"SGF record with id {id} not found.");

                return Ok(record);
            }

            // GET: api/sgfrecord/by-match/{matchId}
            [HttpGet("by-match/{matchId:guid}")]
            public async Task<IActionResult> GetSgfRecordByMatchId(Guid matchId, [FromQuery] bool reload = true)
            {
                var record = await _sgfRepository.GetSgfRecordByMatchId(matchId, reload);

                if (record == null)
                    return NotFound($"SGF record for match {matchId} not found.");

                return Ok(record);
            }

            // POST: api/sgfrecord
            [HttpPost]
            public async Task<IActionResult> CreateSgfRecord([FromBody] SgfRecord newRecord, [FromQuery] bool reload = true)
            {
                if (newRecord == null)
                    return BadRequest("SGF record cannot be null.");

            var moves = _sgfParser.ParseMoves(newRecord.RawSgf); 
            newRecord.ParsedMovesJson = JsonSerializer.Serialize(moves);

            var result = await _sgfRepository.CreateSgfRecord(newRecord, reload);

                return Ok(result);
            }

            // PUT: api/sgfrecord
            [HttpPut]
            public async Task<IActionResult> UpdateSgfRecord([FromBody] SgfRecord updatedRecord, [FromQuery] bool reload = true)
            {
                if (updatedRecord == null)
                    return BadRequest("SGF record cannot be null.");

                var result = await _sgfRepository.UpdateSgfRecord(updatedRecord, reload);

                return Ok(result);
            }

            // DELETE: api/sgfrecord/{id}
            [HttpDelete("{id:guid}")]
            public async Task<IActionResult> DeleteSgfRecord(Guid id, [FromQuery] bool reload = true)
            {
                var existing = await _sgfRepository.GetSgfRecordById(id, reload);

                if (existing == null)
                    return NotFound($"SGF record with id {id} not found.");

                var result = await _sgfRepository.DeleteSgfRecord(existing, reload);

                return Ok(result);
            }

        #region Parse and Replay Endpoints
        [HttpPost("parse")]
            public IActionResult ParseSgf([FromBody] string sgf)
            {
                var moves = _sgfParser.ParseMoves(sgf);
                return Ok(moves);
            }

        [HttpGet("replay/{matchId:guid}")]
        public async Task<IActionResult> ReplayMatch(Guid matchId, [FromQuery] bool reload = true)
        {
            var record = await _sgfRepository.GetSgfRecordByMatchId(matchId, reload);

            if (record == null)
                return NotFound($"SGF record for match {matchId} not found.");

            var moves = _sgfParser.ParseMoves(record.RawSgf);

            return Ok(new
            {
                matchId,
                totalMoves = moves.Count,
                moves
            });
        }

        [HttpGet("replay/{matchId:guid}/move/{moveNumber:int}")]
        public async Task<IActionResult> ReplayMove(Guid matchId, int moveNumber, [FromQuery] bool reload = true)
        {
            var record = await _sgfRepository.GetSgfRecordByMatchId(matchId, reload);

            if (record == null)
                return NotFound($"SGF record for match {matchId} not found.");

            var moves = _sgfParser.ParseMoves(record.RawSgf);

            if (moveNumber < 1 || moveNumber > moves.Count)
                return BadRequest($"Move number must be between 1 and {moves.Count}.");

            return Ok(moves[moveNumber - 1]);
        }

        [HttpGet("replay-stream/{matchId:guid}")]
        public async IAsyncEnumerable<SgfMove> StreamReplay(Guid matchId)
        {
            var record = await _sgfRepository.GetSgfRecordByMatchId(matchId);

            var moves = _sgfParser.ParseMoves(record.RawSgf);

            foreach (var move in moves)
            {
                yield return move;
                await Task.Delay(300); // 300ms between moves
            }
        }
        #endregion Parse and Replay Endpoints
    }
}
