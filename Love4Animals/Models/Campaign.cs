namespace Love4AnimalsApi.Models;

public class Campaign
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal GoalAmount { get; set; }
}
