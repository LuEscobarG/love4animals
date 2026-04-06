using Love4AnimalsApi.Dtos;

namespace Love4AnimalsApi.Interfaces;

public interface IPublicationService
{
    List<GetPublicationDto> GetPublications();
    GetPublicationDto? GetPublicationById(int id);
    GetPublicationDto CreatePublication(CreatePublicationDto dto);
    GetPublicationDto? UpdatePublication(int id, UpdatePublicationDto dto);
    bool DeletePublication(int id);
    bool AddLike(int id);
    bool AddShare(int id);
    bool AddComment(int id, CreateCommentDto dto);
}

