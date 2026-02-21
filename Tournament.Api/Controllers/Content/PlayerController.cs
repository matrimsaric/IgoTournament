using Microsoft.AspNetCore.Mvc;
using PlayerDomain.Model;
using PlayerDomain.Services.Interfaces;
using Image = ImageDomain.Model.Image;

namespace Tournament.Api.Controllers.Content
{
    [ApiController]
    [Route("api/content/players")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _service;

        public PlayerController(IPlayerService service)
        {
            _service = service;
        }

        // GET: api/content/players
        [HttpGet]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await _service.GetAllPlayersAsync();
            return Ok(players);
        }

        // GET: api/content/players/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPlayer(Guid id)
        {
            var player = await _service.GetPlayerByIdAsync(id);
            return player is null ? NotFound() : Ok(player);
        }

        // POST: api/content/players
        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromBody] Player newPlayer)
        {
            var result = await _service.CreatePlayerAsync(newPlayer);
            return Ok(result);
        }

        // PUT: api/content/players/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePlayer(Guid id, [FromBody] Player updatedPlayer)
        {
            updatedPlayer.Id = id;
            var result = await _service.UpdatePlayerAsync(updatedPlayer);
            return Ok(result);
        }

        // DELETE: api/content/players/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePlayer(Guid id)
        {
            var result = await _service.DeletePlayerAsync(id);
            return Ok(result);
        }

        // IMAGES
        [HttpGet("{id:guid}/images")]
        public async Task<IActionResult> GetImages(Guid id)
        {
            var images = await _service.GetImagesForPlayerAsync(id);
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
