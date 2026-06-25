namespace Love4AnimalsApi.Dtos;

public record CreateDonationDto(
    int UserId,
    int CampaignId,
    decimal Amount
);
