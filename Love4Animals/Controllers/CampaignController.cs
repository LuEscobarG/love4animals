using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Love4AnimalsApi.Controllers
{
    [Route("v1/campaigns")]
    [ApiController]
    [Authorize]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            this.campaignService = campaignService;
        }

        [HttpGet]
        [OutputCache(PolicyName = "Default", Tags = ["campaigns"])]
        [EndpointSummary("Obtener todas las campañas")]
        [ProducesResponseType<List<GetCampaignDto>>(200)]
        public ActionResult<List<GetCampaignDto>> GetCampaigns()
        {
            return Ok(campaignService.GetCampaigns());
        }

        [HttpGet("{id}")]
        [OutputCache(PolicyName = "Default", Tags = ["campaigns"])]
        [EndpointSummary("Obtener campaña por ID")]
        [ProducesResponseType<GetCampaignDto>(200)]
        [ProducesResponseType(404)]
        public ActionResult<GetCampaignDto> GetCampaignById(int id)
        {
            var campaign = campaignService.GetCampaignById(id);
            if (campaign == null) return NotFound(new { message = "Campaña no encontrada" });
            return Ok(campaign);
        }

        [HttpPost]
        [Authorize(Roles = "Missionary")]
        [EndpointSummary("Crear campaña (solo misioneros)")]
        [Consumes("application/json")]
        [ProducesResponseType<GetCampaignDto>(201)]
        [ProducesResponseType<ValidationProblemDetails>(400)]
        public ActionResult<GetCampaignDto> CreateCampaign([FromBody] CreateCampaignDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var createdCampaign = campaignService.CreateCampaign(dto);
            return CreatedAtAction(nameof(GetCampaignById), new { id = createdCampaign.Id }, createdCampaign);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Missionary")]
        [EndpointSummary("Actualizar campaña")]
        [Consumes("application/json")]
        [ProducesResponseType<GetCampaignDto>(200)]
        [ProducesResponseType<ValidationProblemDetails>(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCampaign(int id, [FromBody] UpdateCampaignDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = campaignService.UpdateCampaign(id, dto);
            if (updated == null) return NotFound(new { message = "Campaña no encontrada" });
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Missionary")]
        [EndpointSummary("Eliminar campaña")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCampaign(int id)
        {
            var deleted = campaignService.DeleteCampaign(id);
            if (!deleted) return NotFound(new { message = "Campaña no encontrada" });
            return NoContent();
        }
    }
}
