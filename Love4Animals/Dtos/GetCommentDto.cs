namespace Love4AnimalsApi.Dtos;

public record GetCommentDto(
    int Id,
    int UserId,
    string UserName,
    string Content
);
