namespace Love4AnimalsApi.Dtos;

public record UpdateCampaignDto(
    string Title,
    string Description,
    decimal GoalAmount
);