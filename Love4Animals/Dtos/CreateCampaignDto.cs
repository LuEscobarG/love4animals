namespace Love4AnimalsApi.Dtos;

public record CreateCampaignDto(
    string Title,
    string Description,
    decimal GoalAmount
);