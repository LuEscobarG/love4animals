namespace Love4AnimalsApi.Dtos;

public record GetCampaignDto(
    int Id,
    string Title,
    string Description,
    decimal GoalAmount
);