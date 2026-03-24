using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Love4AnimalsApi.Controllers
{
    [Route("v1/campaigns")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private ICampaignService campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            this.campaignService = campaignService;
        }

        [HttpGet]
        public ActionResult<List<GetCampaignDto>> GetCampaigns()
        {
            return Ok(campaignService.GetCampaigns());
        }

        [HttpGet("{id}")]
        public ActionResult<GetCampaignDto> GetCampaignById(int id)
        {
            var campaign = campaignService.GetCampaignById(id);

            if (campaign == null)
                return NotFound(new { message = "Campaña no encontrada" });

            return Ok(campaign);
        }

        [HttpPost]
        public ActionResult<GetCampaignDto> CreateCampaign([FromBody] CreateCampaignDto dto)
        {
            var createdCampaign = campaignService.CreateCampaign(dto);
            return CreatedAtAction(nameof(GetCampaignById), new { id = createdCampaign.Id }, createdCampaign);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCampaign(int id, [FromBody] UpdateCampaignDto dto)
        {
            var updated = campaignService.UpdateCampaign(id, dto);

            if (!updated)
                return NotFound(new { message = "Campaña no encontrada" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCampaign(int id)
        {
            var deleted = campaignService.DeleteCampaign(id);

            if (!deleted)
                return NotFound(new { message = "Campaña no encontrada" });

            return NoContent();
        }
    }
}