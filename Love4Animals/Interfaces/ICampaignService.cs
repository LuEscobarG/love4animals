using Love4AnimalsApi.Dtos;

namespace Love4AnimalsApi.Interfaces;

public interface ICampaignService
{
    List<GetCampaignDto> GetCampaigns();
    GetCampaignDto? GetCampaignById(int id);
    GetCampaignDto CreateCampaign(CreateCampaignDto dto);
    bool UpdateCampaign(int id, UpdateCampaignDto dto);
    bool DeleteCampaign(int id);
}