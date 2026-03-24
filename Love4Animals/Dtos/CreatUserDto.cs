namespace Love4AnimalsApi.Dtos;

public record CreateUserDto(
    int Id,
    string Name,
    string Email
);