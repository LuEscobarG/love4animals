using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Services;

public class CampaignService : ICampaignService
{
    private readonly ICampaignRepository _campaignRepository;

    public CampaignService(ICampaignRepository campaignRepository)
    {
        _campaignRepository = campaignRepository;
    }

    public List<GetCampaignDto> GetCampaigns() =>
        _campaignRepository.GetCampaigns()
            .Select(c => new GetCampaignDto(c.Id, c.Title, c.Description, c.GoalAmount))
            .ToList();

    public GetCampaignDto? GetCampaignById(int id)
    {
        var campaign = _campaignRepository.GetCampaignById(id);
        return campaign == null ? null : new GetCampaignDto(campaign.Id, campaign.Title, campaign.Description, campaign.GoalAmount);
    }

    public GetCampaignDto CreateCampaign(CreateCampaignDto dto)
    {
        var newCampaign = new Campaign
        {
            Title = dto.Title,
            Description = dto.Description,
            GoalAmount = dto.GoalAmount
        };
        var created = _campaignRepository.CreateCampaign(newCampaign);
        return new GetCampaignDto(created.Id, created.Title, created.Description, created.GoalAmount);
    }

    public GetCampaignDto? UpdateCampaign(int id, UpdateCampaignDto dto)
    {
        var campaign = new Campaign { Id = id, Title = dto.Title, Description = dto.Description, GoalAmount = dto.GoalAmount };
        var success = _campaignRepository.UpdateCampaign(id, campaign);
        return success ? new GetCampaignDto(id, dto.Title, dto.Description, dto.GoalAmount) : null;
    }

    public bool DeleteCampaign(int id) =>
        _campaignRepository.DeleteCampaign(id);
}
