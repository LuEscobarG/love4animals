using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Love4AnimalsApi.Controllers
{
    [Route("v1/publications")]
    [ApiController]
    public class PublicationController : ControllerBase
    {
        private IPublicationService publicationService;

        public PublicationController(IPublicationService publicationService)
        {
            this.publicationService = publicationService;
        }

        [HttpGet]
        public ActionResult<List<GetPublicationDto>> GetPublications()
        {
            return Ok(publicationService.GetPublications());
        }

        [HttpGet("{id}")]
        public ActionResult<GetPublicationDto> GetPublicationById(int id)
        {
            var publication = publicationService.GetPublicationById(id);

            if (publication == null)
                return NotFound(new { message = "Publicación no encontrada" });

            return Ok(publication);
        }

        [HttpPost]
        public ActionResult<GetPublicationDto> CreatePublication([FromBody] CreatePublicationDto dto)
        {
            var created = publicationService.CreatePublication(dto);
            return CreatedAtAction(nameof(GetPublicationById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePublication(int id, [FromBody] UpdatePublicationDto dto)
        {
            var updated = publicationService.UpdatePublication(id, dto);

            if (updated == null)
                return NotFound(new { message = "Publicación no encontrada" });

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePublication(int id)
        {
            var deleted = publicationService.DeletePublication(id);

            if (!deleted)
                return NotFound(new { message = "Publicación no encontrada" });

            return NoContent();
        }

        [HttpPost("{id}/likes")]
        public IActionResult AddLike(int id)
        {
            var result = publicationService.AddLike(id);

            if (!result)
                return NotFound(new { message = "Publicación no encontrada" });

            return NoContent();
        }

        [HttpPost("{id}/shares")]
        public IActionResult AddShare(int id)
        {
            var result = publicationService.AddShare(id);

            if (!result)
                return NotFound(new { message = "Publicación no encontrada" });

            return NoContent();
        }

        [HttpPost("{id}/comments")]
        public IActionResult AddComment(int id, [FromBody] CreateCommentDto dto)
        {
            var result = publicationService.AddComment(id, dto);

            if (!result)
                return NotFound(new { message = "Publicación no encontrada" });

            return NoContent();
        }
    }
}
