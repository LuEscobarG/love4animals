namespace Love4AnimalsApi.Dtos;

public record LoginResponseDto(
    int Id,
    string Name,
    string Email,
    string UserType,
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiry
);
