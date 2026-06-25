using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Love4AnimalsApi.Controllers
{
    [Route("v1/donations")]
    [ApiController]
    [Authorize]
    public class DonationController : ControllerBase
    {
        private readonly IDonationService donationService;

        public DonationController(IDonationService donationService)
        {
            this.donationService = donationService;
        }

        [HttpGet]
        [OutputCache(PolicyName = "Default", Tags = ["donations"])]
        [EndpointSummary("Obtener todas las donaciones")]
        [ProducesResponseType<List<GetDonationDto>>(200)]
        public ActionResult<List<GetDonationDto>> GetDonations()
        {
            return Ok(donationService.GetDonations());
        }

        [HttpGet("{id}")]
        [OutputCache(PolicyName = "Default", Tags = ["donations"])]
        [EndpointSummary("Obtener donación por ID")]
        [ProducesResponseType<GetDonationDto>(200)]
        [ProducesResponseType(404)]
        public ActionResult<GetDonationDto> GetDonationById(int id)
        {
            var donation = donationService.GetDonationById(id);
            if (donation == null) return NotFound(new { message = "Donación no encontrada" });
            return Ok(donation);
        }

        [HttpPost]
        [Authorize(Roles = "Donor")]
        [EndpointSummary("Crear donación (solo donantes)")]
        [Consumes("application/json")]
        [ProducesResponseType<GetDonationDto>(201)]
        [ProducesResponseType<ValidationProblemDetails>(400)]
        [ProducesResponseType(403)]
        public ActionResult<GetDonationDto> CreateDonation([FromBody] CreateDonationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = donationService.CreateDonation(dto);
            if (created == null) return StatusCode(403, new { message = "Solo los donantes pueden realizar donaciones" });
            return CreatedAtAction(nameof(GetDonationById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [EndpointSummary("Actualizar donación")]
        [Consumes("application/json")]
        [ProducesResponseType<GetDonationDto>(200)]
        [ProducesResponseType<ValidationProblemDetails>(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateDonation(int id, [FromBody] UpdateDonationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = donationService.UpdateDonation(id, dto);
            if (updated == null) return NotFound(new { message = "Donación no encontrada" });
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Eliminar donación")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteDonation(int id)
        {
            var deleted = donationService.DeleteDonation(id);
            if (!deleted) return NotFound(new { message = "Donación no encontrada" });
            return NoContent();
        }
    }
}
