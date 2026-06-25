namespace Love4AnimalsApi.Dtos;

public record UpdatePublicationDto(
    string ImageUrl,
    string Content,
    List<int> CampaignIds,
    int Likes,
    int Shares
);
