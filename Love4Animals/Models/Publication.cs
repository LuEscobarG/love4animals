namespace Love4AnimalsApi.Models;

public class Publication
{
    public Publication(int id, string content, List<int> campaignIds)
    {
        Id = id;
        Content = content;
        CampaignIds = campaignIds;
        Likes = 0;
        Shares = 0;
        Comments = new List<Comment>();
    }

    public int Id { get; set; }
    public string Content { get; set; }
    public List<int> CampaignIds { get; set; }
    public int Likes { get; set; }
    public int Shares { get; set; }
    public List<Comment> Comments { get; set; }
}
