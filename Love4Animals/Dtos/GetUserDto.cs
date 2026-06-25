namespace Love4AnimalsApi.Dtos;

public record GetUserDto(
    int Id,
    string Name,
    string Email,
    string PasswordHash,
    string UserType,
    string Phone,
    string Bio
);
