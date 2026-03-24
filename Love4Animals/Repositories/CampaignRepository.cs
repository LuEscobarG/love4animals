using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Repositories;

public class CampaignRepository : ICampaignRepository
{
    private List<Campaign> Campaigns { get; set; }

    public CampaignRepository()
    {
        Campaigns = new List<Campaign>();

        Campaigns.Add(new Campaign(1, "Rescate de fauna", "Campaña para rescatar animales silvestres", 5000));
        Campaigns.Add(new Campaign(2, "Alimento para refugio", "Campaña para reunir alimento", 3000));
    }

    public List<Campaign> GetCampaigns()
    {
        return Campaigns;
    }

    public Campaign? GetCampaignById(int id)
    {
        return Campaigns.FirstOrDefault(c => c.Id == id);
    }

    public Campaign CreateCampaign(Campaign campaign)
    {
        Campaigns.Add(campaign);
        return campaign;
    }

    public bool UpdateCampaign(int id, Campaign campaign)
    {
        var existingCampaign = Campaigns.FirstOrDefault(c => c.Id == id);

        if (existingCampaign == null)
            return false;

        existingCampaign.Title = campaign.Title;
        existingCampaign.Description = campaign.Description;
        existingCampaign.GoalAmount = campaign.GoalAmount;

        return true;
    }

    public bool DeleteCampaign(int id)
    {
        var campaign = Campaigns.FirstOrDefault(c => c.Id == id);

        if (campaign == null)
            return false;

        Campaigns.Remove(campaign);
        return true;
    }
}