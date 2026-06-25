namespace Love4AnimalsApi.Dtos;

public record CreatePublicationDto(
    int UserId,
    string ImageUrl,
    string Content,
    List<int> CampaignIds
);