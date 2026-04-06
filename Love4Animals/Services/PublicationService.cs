using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Services;

public class PublicationService : IPublicationService
{
    private IPublicationRepository publicationRepository;
    private IUserRepository userRepository;

    public PublicationService(IPublicationRepository publicationRepository, IUserRepository userRepository)
    {
        this.publicationRepository = publicationRepository;
        this.userRepository = userRepository;
    }

    private GetPublicationDto MapToDto(Publication p)
    {
        var comments = p.Comments.Select(c =>
        {
            var user = userRepository.GetUserById(c.UserId);
            return new GetCommentDto(c.Id, c.UserId, user?.Name ?? "Unknown", c.Content);
        }).ToList();

        return new GetPublicationDto(p.Id, p.Content, p.CampaignIds, p.Likes, p.Shares, p.Comments.Count, comments);
    }

    public List<GetPublicationDto> GetPublications()
    {
        return publicationRepository.GetPublications().Select(MapToDto).ToList();
    }

    public GetPublicationDto? GetPublicationById(int id)
    {
        var publication = publicationRepository.GetPublicationById(id);

        if (publication == null)
            return null;

        return MapToDto(publication);
    }

    public GetPublicationDto CreatePublication(CreatePublicationDto dto)
    {
        var publications = publicationRepository.GetPublications();
        int newId = publications.Count > 0 ? publications.Max(p => p.Id) + 1 : 1;

        var newPublication = new Publication(newId, dto.Content, dto.CampaignIds);
        var created = publicationRepository.CreatePublication(newPublication);

        return MapToDto(created);
    }

    public GetPublicationDto? UpdatePublication(int id, UpdatePublicationDto dto)
    {
        var updated = new Publication(id, dto.Content, dto.CampaignIds)
        {
            Likes = dto.Likes,
            Shares = dto.Shares
        };
        var success = publicationRepository.UpdatePublication(id, updated);

        if (!success)
            return null;

        return GetPublicationById(id);
    }

    public bool DeletePublication(int id)
    {
        return publicationRepository.DeletePublication(id);
    }

    public bool AddLike(int id)
    {
        return publicationRepository.AddLike(id);
    }

    public bool AddShare(int id)
    {
        return publicationRepository.AddShare(id);
    }

    public bool AddComment(int id, CreateCommentDto dto)
    {
        var publication = publicationRepository.GetPublicationById(id);

        if (publication == null)
            return false;

        int newCommentId = publication.Comments.Count > 0 ? publication.Comments.Max(c => c.Id) + 1 : 1;
        var comment = new Comment(newCommentId, dto.UserId, dto.Content);

        return publicationRepository.AddComment(id, comment);
    }
}
