namespace Love4AnimalsApi.Dtos;

public record GetPublicationDto(
    int Id,
    int UserId,
    string UserName,
    string ImageUrl,
    string Content,
    List<int> CampaignIds,
    int Likes,
    int Shares,
    int CommentCount,
    List<GetCommentDto> Comments
);

