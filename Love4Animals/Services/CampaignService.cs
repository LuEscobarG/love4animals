using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Services;

public class CampaignService : ICampaignService
{
    private ICampaignRepository campaignRepository;

    public CampaignService(ICampaignRepository campaignRepository)
    {
        this.campaignRepository = campaignRepository;
    }

    public List<GetCampaignDto> GetCampaigns()
    {
        return campaignRepository.GetCampaigns()
            .Select(c => new GetCampaignDto(c.Id, c.Title, c.Description, c.GoalAmount))
            .ToList();
    }

    public GetCampaignDto? GetCampaignById(int id)
    {
        var campaign = campaignRepository.GetCampaignById(id);

        if (campaign == null)
            return null;

        return new GetCampaignDto(campaign.Id, campaign.Title, campaign.Description, campaign.GoalAmount);
    }

    public GetCampaignDto CreateCampaign(CreateCampaignDto dto)
    {
        var campaigns = campaignRepository.GetCampaigns();
        int newId = campaigns.Count > 0 ? campaigns.Max(c => c.Id) + 1 : 1;

        Campaign newCampaign = new Campaign(newId, dto.Title, dto.Description, dto.GoalAmount);
        var createdCampaign = campaignRepository.CreateCampaign(newCampaign);

        return new GetCampaignDto(
            createdCampaign.Id,
            createdCampaign.Title,
            createdCampaign.Description,
            createdCampaign.GoalAmount
        );
    }

    public bool UpdateCampaign(int id, UpdateCampaignDto dto)
    {
        Campaign updatedCampaign = new Campaign(id, dto.Title, dto.Description, dto.GoalAmount);
        return campaignRepository.UpdateCampaign(id, updatedCampaign);
    }

    public bool DeleteCampaign(int id)
    {
        return campaignRepository.DeleteCampaign(id);
    }
}