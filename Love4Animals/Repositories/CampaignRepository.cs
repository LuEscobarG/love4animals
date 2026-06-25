using Love4AnimalsApi.Data;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Love4AnimalsApi.Repositories;

public class CampaignRepository : ICampaignRepository
{
    private readonly AppDbContext _context;

    public CampaignRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Campaign> GetCampaigns() =>
        _context.Campaigns.ToList();

    public Campaign? GetCampaignById(int id) =>
        _context.Campaigns.FirstOrDefault(c => c.Id == id);

    public Campaign CreateCampaign(Campaign campaign)
    {
        _context.Campaigns.Add(campaign);
        _context.SaveChanges();
        return campaign;
    }

    public bool UpdateCampaign(int id, Campaign campaign)
    {
        var existing = _context.Campaigns.FirstOrDefault(c => c.Id == id);
        if (existing == null) return false;

        existing.Title = campaign.Title;
        existing.Description = campaign.Description;
        existing.GoalAmount = campaign.GoalAmount;

        _context.SaveChanges();
        return true;
    }

    public bool DeleteCampaign(int id)
    {
        var campaign = _context.Campaigns.FirstOrDefault(c => c.Id == id);
        if (campaign == null) return false;

        _context.Campaigns.Remove(campaign);
        _context.SaveChanges();
        return true;
    }
}
