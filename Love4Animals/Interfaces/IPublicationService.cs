using Love4AnimalsApi.Dtos;

namespace Love4AnimalsApi.Interfaces;

public interface IPublicationService
{
    List<GetPublicationDto> GetPublications();
    GetPublicationDto? GetPublicationById(int id);
    GetPublicationDto? CreatePublication(CreatePublicationDto dto);
    GetPublicationDto? UpdatePublication(int id, UpdatePublicationDto dto);
    bool DeletePublication(int id);
    bool AddLike(int id);
    bool AddShare(int id);
    GetCommentDto? AddComment(int publicationId, CreateCommentDto dto);
    GetCommentDto? UpdateComment(int publicationId, int commentId, UpdateCommentDto dto);
    bool DeleteComment(int publicationId, int commentId);
}
