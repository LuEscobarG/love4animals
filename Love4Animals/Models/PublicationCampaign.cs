namespace Love4AnimalsApi.Models;

// Tabla de unión para la relación many-to-many entre Publication y Campaign
public class PublicationCampaign
{
    public int PublicationId { get; set; }
    public Publication Publication { get; set; } = null!;

    public int CampaignId { get; set; }
    public Campaign Campaign { get; set; } = null!;
}
