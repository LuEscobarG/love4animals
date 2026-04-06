namespace Love4AnimalsApi.Dtos;

public record CreatePublicationDto(
    string Content,
    List<int> CampaignIds
);
