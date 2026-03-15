using Microsoft.AspNetCore.Mvc;
using ImageDomain.ControlModule.Interfaces;
using ImageDomain.Model;

namespace Tournament.Api.Controllers.Content
{
    [ApiController]
    [Route("api/content/images")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _service;

        public ImageController(IImageService service)
        {
            _service = service;
        }

        // GET: api/content/images/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var image = await _service.GetImageById(id);
            return image is null ? NotFound() : Ok(image);
        }

        // GET: api/content/images/object/{objectId}/{objectType}
        [HttpGet("object/{objectId:guid}/{objectType:int}")]
        public async Task<IActionResult> GetImagesForObject(Guid objectId, int objectType)
        {
            var images = await _service.GetImagesForObject(objectId, objectType);
            return Ok(images);
        }

        // GET: api/content/images/object/{objectId}/{objectType}/primary
        [HttpGet("object/{objectId:guid}/{objectType:int}/primary")]
        public async Task<IActionResult> GetPrimaryImage(Guid objectId, int objectType)
        {
            var image = await _service.GetPrimaryImageForObject(objectId, objectType);
            return image is null ? NotFound() : Ok(image);
        }

        // POST: api/content/images
        [HttpPost]
        public async Task<IActionResult> AddImage([FromBody] Image newImage)
        {
            var result = await _service.AddImage(newImage);
            return Ok(result);
        }

        // PUT: api/content/images/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateImage(Guid id, [FromBody] Image updatedImage)
        {
            updatedImage.Id = id;
            var result = await _service.UpdateImage(updatedImage);
            return Ok(result);
        }

        // DELETE: api/content/images/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            var image = await _service.GetImageById(id);
            if (image is null)
                return NotFound();

            var result = await _service.DeleteImage(image);
            return Ok(result);
        }
    }
}
