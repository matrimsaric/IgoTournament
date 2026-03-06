using Microsoft.AspNetCore.Mvc;
using CompetitionDomain.Model;
using CompetitionDomain.Services.Interfaces;

namespace Tournament.Api.Controllers.Content
{
    [ApiController]
    [Route("api/content/sgf-records")]
    public class SgfRecordController : ControllerBase
    {
        private readonly ISgfRecordService _service;

        public SgfRecordController(ISgfRecordService service)
        {
            _service = service;
        }

        // GET: api/content/sgf-records
        [HttpGet]
        public async Task<IActionResult> GetAllSgfRecords()
        {
            var records = await _service.GetAllSgfRecordsAsync();
            return Ok(records);
        }

        // GET: api/content/sgf-records/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSgfRecord(Guid id)
        {
            var record = await _service.GetSgfRecordByIdAsync(id);
            return record is null ? NotFound() : Ok(record);
        }

        // GET: api/content/sgf-records/by-match/{matchId}
        [HttpGet("by-match/{matchId:guid}")]
        public async Task<IActionResult> GetSgfRecordByMatch(Guid matchId)
        {
            var record = await _service.GetSgfRecordByMatchIdAsync(matchId);
            return record is null ? NotFound() : Ok(record);
        }

        // POST: api/content/sgf-records
        [HttpPost]
        public async Task<IActionResult> CreateSgfRecord([FromBody] SgfRecord newRecord)
        {
            var result = await _service.CreateSgfRecordAsync(newRecord);
            return Ok(result);
        }

        // PUT: api/content/sgf-records/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSgfRecord(Guid id, [FromBody] SgfRecord updatedRecord)
        {
            updatedRecord.Id = id;
            var result = await _service.UpdateSgfRecordAsync(updatedRecord);
            return Ok(result);
        }

        // DELETE: api/content/sgf-records/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSgfRecord(Guid id)
        {
            var result = await _service.DeleteSgfRecordAsync(id);
            return Ok(result);
        }
    }
}
