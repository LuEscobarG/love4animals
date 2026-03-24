namespace Love4AnimalsApi.Models;

public class Campaign
{
    public Campaign(int id, string title, string description, decimal goalAmount)
    {
        Id = id;
        Title = title;
        Description = description;
        GoalAmount = goalAmount;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal GoalAmount { get; set; }
}