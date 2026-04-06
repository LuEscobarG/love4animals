namespace Love4AnimalsApi.Dtos;

public record CreateCommentDto(
    int UserId,
    string Content
);
