using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Interfaces;

public interface ICampaignRepository
{
    List<Campaign> GetCampaigns();
    Campaign? GetCampaignById(int id);
    Campaign CreateCampaign(Campaign campaign);
    bool UpdateCampaign(int id, Campaign campaign);
    bool DeleteCampaign(int id);
}