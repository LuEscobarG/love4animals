using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Interfaces;

public interface IPublicationRepository
{
    List<Publication> GetPublications();
    Publication? GetPublicationById(int id);
    Publication CreatePublication(Publication publication);
    bool UpdatePublication(int id, Publication publication);
    bool DeletePublication(int id);
    bool AddLike(int id);
    bool AddShare(int id);
    bool AddComment(int publicationId, Comment comment);
    bool UpdateComment(int publicationId, int commentId, string content);
    bool DeleteComment(int publicationId, int commentId);
}
