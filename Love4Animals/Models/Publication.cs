namespace Love4AnimalsApi.Models;

public class Publication
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Likes { get; set; }
    public int Shares { get; set; }

    // Relaciones
    public List<Comment> Comments { get; set; } = new();
    public List<PublicationCampaign> PublicationCampaigns { get; set; } = new();
}
