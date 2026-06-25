namespace Love4AnimalsApi.Models;

public class Donation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CampaignId { get; set; }
    public decimal Amount { get; set; }
}
