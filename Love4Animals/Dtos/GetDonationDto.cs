namespace Love4AnimalsApi.Dtos;

public record GetDonationDto(
    int Id,
    int UserId,
    string UserName,
    int CampaignId,
    string CampaignTitle,
    decimal Amount
);
