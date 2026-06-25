using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Love4AnimalsApi.Controllers
{
    [Route("v1/publications")]
    [ApiController]
    [Authorize]
    public class PublicationController : ControllerBase
    {
        private readonly IPublicationService publicationService;

        public PublicationController(IPublicationService publicationService)
        {
            this.publicationService = publicationService;
        }

        [HttpGet]
        [OutputCache(PolicyName = "Default", Tags = ["publications"])]
        [EndpointSummary("Obtener todas las publicaciones")]
        [ProducesResponseType<List<GetPublicationDto>>(200)]
        public ActionResult<List<GetPublicationDto>> GetPublications()
        {
            return Ok(publicationService.GetPublications());
        }

        [HttpGet("{id}")]
        [OutputCache(PolicyName = "Default", Tags = ["publications"])]
        [EndpointSummary("Obtener publicación por ID")]
        [ProducesResponseType<GetPublicationDto>(200)]
        [ProducesResponseType(404)]
        public ActionResult<GetPublicationDto> GetPublicationById(int id)
        {
            var publication = publicationService.GetPublicationById(id);
            if (publication == null) return NotFound(new { message = "Publicación no encontrada" });
            return Ok(publication);
        }

        [HttpPost]
        [Authorize(Roles = "Missionary")]
        [EndpointSummary("Crear publicación (solo misioneros)")]
        [Consumes("application/json")]
        [ProducesResponseType<GetPublicationDto>(201)]
        [ProducesResponseType<ValidationProblemDetails>(400)]
        [ProducesResponseType(403)]
        public ActionResult<GetPublicationDto> CreatePublication([FromBody] CreatePublicationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = publicationService.CreatePublication(dto);
            if (created == null) return StatusCode(403, new { message = "Solo los misioneros pueden crear publicaciones" });
            return CreatedAtAction(nameof(GetPublicationById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Missionary")]
        [EndpointSummary("Actualizar publicación")]
        [Consumes("application/json")]
        [ProducesResponseType<GetPublicationDto>(200)]
        [ProducesResponseType<ValidationProblemDetails>(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePublication(int id, [FromBody] UpdatePublicationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = publicationService.UpdatePublication(id, dto);
            if (updated == null) return NotFound(new { message = "Publicación no encontrada" });
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Missionary")]
        [EndpointSummary("Eliminar publicación")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePublication(int id)
        {
            var deleted = publicationService.DeletePublication(id);
            if (!deleted) return NotFound(new { message = "Publicación no encontrada" });
            return NoContent();
        }

        [HttpPost("{id}/likes")]
        [EndpointSummary("Agregar like a publicación")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult AddLike(int id)
        {
            var result = publicationService.AddLike(id);
            if (!result) return NotFound(new { message = "Publicación no encontrada" });
            return NoContent();
        }

        [HttpPost("{id}/shares")]
        [EndpointSummary("Agregar share a publicación")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult AddShare(int id)
        {
            var result = publicationService.AddShare(id);
            if (!result) return NotFound(new { message = "Publicación no encontrada" });
            return NoContent();
        }

        [HttpPost("{postId}/comments")]
        [EndpointSummary("Agregar comentario a publicación")]
        [Consumes("application/json")]
        [ProducesResponseType<GetCommentDto>(200)]
        [ProducesResponseType<ValidationProblemDetails>(400)]
        [ProducesResponseType(404)]
        public ActionResult<GetCommentDto> AddComment(int postId, [FromBody] CreateCommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comment = publicationService.AddComment(postId, dto);
            if (comment == null) return NotFound(new { message = "Publicación no encontrada" });
            return Ok(comment);
        }

        [HttpPut("{postId}/comments/{commentId}")]
        [EndpointSummary("Actualizar comentario")]
        [Consumes("application/json")]
        [ProducesResponseType<GetCommentDto>(200)]
        [ProducesResponseType<ValidationProblemDetails>(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateComment(int postId, int commentId, [FromBody] UpdateCommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comment = publicationService.UpdateComment(postId, commentId, dto);
            if (comment == null) return NotFound(new { message = "Comentario o publicación no encontrada" });
            return Ok(comment);
        }

        [HttpDelete("{postId}/comments/{commentId}")]
        [EndpointSummary("Eliminar comentario")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteComment(int postId, int commentId)
        {
            var deleted = publicationService.DeleteComment(postId, commentId);
            if (!deleted) return NotFound(new { message = "Comentario o publicación no encontrada" });
            return NoContent();
        }
    }
}
