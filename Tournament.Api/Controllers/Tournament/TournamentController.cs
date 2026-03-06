using CompetitionDomain.Model;
using CompetitionDomain.Services;
using CompetitionDomain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Image = ImageDomain.Model.Image;

namespace Tournament.Api.Controllers.Content
{
    [ApiController]
    [Route("api/content/tournaments")]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService _service;
        private readonly IRoundService _roundService;

        public TournamentController(ITournamentService service, IRoundService roundService)
        {
            _service = service;
            _roundService = roundService;
        }

        // GET: api/content/tournaments
        [HttpGet]
        public async Task<IActionResult> GetAllTournaments()
        {
            var tournaments = await _service.GetAllTournamentsAsync();
            return Ok(tournaments);
        }

        // GET: api/content/tournaments/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTournament(Guid id)
        {
            var tournament = await _service.GetTournamentByIdAsync(id);
            return tournament is null ? NotFound() : Ok(tournament);
        }

        // POST: api/content/tournaments
        [HttpPost]
        public async Task<IActionResult> CreateTournament([FromBody] CompetitionDomain.Model.Tournament newTournament)
        {
            var result = await _service.CreateTournamentAsync(newTournament);
            return Ok(result);
        }

        // PUT: api/content/tournaments/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTournament(Guid id, [FromBody] CompetitionDomain.Model.Tournament updatedTournament)
        {
            updatedTournament.Id = id;
            var result = await _service.UpdateTournamentAsync(updatedTournament);
            return Ok(result);
        }

        // DELETE: api/content/tournaments/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTournament(Guid id)
        {
            var result = await _service.DeleteTournamentAsync(id);
            return Ok(result);
        }

        // GET: api/content/tournaments/{id}/rounds
        [HttpGet("{id:guid}/rounds")]
        public async Task<IActionResult> GetRoundsForTournament(Guid id)
        {
            // Get all rounds
            var rounds = await _roundService.GetAllRoundsAsync();

            // Filter by tournament ID
            var filtered = rounds
                .Where(r => r.TournamentId == id)
                .ToList();

            return Ok(filtered);
        }


        // IMAGES
        [HttpGet("{id:guid}/images")]
        public async Task<IActionResult> GetImages(Guid id)
        {
            var images = await _service.GetImagesForTournamentAsync(id);
            return Ok(images);
        }

        [HttpGet("images/{imageId:guid}")]
        public async Task<IActionResult> GetImage(Guid imageId)
        {
            var image = await _service.GetImageByIdAsync(imageId);
            return image is null ? NotFound() : Ok(image);
        }

        [HttpGet("{id:guid}/images/primary")]
        public async Task<IActionResult> GetPrimaryImage(Guid id)
        {
            var image = await _service.GetPrimaryImageAsync(id);
            return image is null ? NotFound() : Ok(image);
        }

        [HttpPost("{id:guid}/images")]
        public async Task<IActionResult> AddImage(Guid id, [FromBody] Image newImage)
        {
            var result = await _service.AddImageAsync(id, newImage);
            return Ok(result);
        }

        [HttpPut("images/{imageId:guid}")]
        public async Task<IActionResult> UpdateImage(Guid imageId, [FromBody] Image updatedImage)
        {
            updatedImage.Id = imageId;
            var result = await _service.UpdateImageAsync(updatedImage);
            return Ok(result);
        }

        [HttpDelete("images/{imageId:guid}")]
        public async Task<IActionResult> DeleteImage(Guid imageId)
        {
            var result = await _service.DeleteImageAsync(imageId);
            return Ok(result);
        }
    }
}
