using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Services;

public class PublicationService : IPublicationService
{
    private readonly IPublicationRepository _publicationRepository;
    private readonly IUserRepository _userRepository;

    public PublicationService(IPublicationRepository publicationRepository, IUserRepository userRepository)
    {
        _publicationRepository = publicationRepository;
        _userRepository = userRepository;
    }

    private GetPublicationDto MapToDto(Publication p)
    {
        var author = _userRepository.GetUserById(p.UserId);
        var campaignIds = p.PublicationCampaigns.Select(pc => pc.CampaignId).ToList();

        var comments = p.Comments.Select(c =>
        {
            var user = _userRepository.GetUserById(c.UserId);
            return new GetCommentDto(c.Id, c.UserId, user?.Name ?? "Unknown", c.Content);
        }).ToList();

        return new GetPublicationDto(
            p.Id, p.UserId, author?.Name ?? "Unknown",
            p.ImageUrl, p.Content, campaignIds,
            p.Likes, p.Shares, p.Comments.Count, comments
        );
    }

    public List<GetPublicationDto> GetPublications() =>
        _publicationRepository.GetPublications().Select(MapToDto).ToList();

    public GetPublicationDto? GetPublicationById(int id)
    {
        var publication = _publicationRepository.GetPublicationById(id);
        return publication == null ? null : MapToDto(publication);
    }

    public GetPublicationDto? CreatePublication(CreatePublicationDto dto)
    {
        var user = _userRepository.GetUserById(dto.UserId);
        if (user == null || user.UserType != UserType.Missionary) return null;

        var publication = new Publication
        {
            UserId = dto.UserId,
            ImageUrl = dto.ImageUrl,
            Content = dto.Content,
            PublicationCampaigns = dto.CampaignIds
                .Select(cid => new PublicationCampaign { CampaignId = cid })
                .ToList()
        };

        var created = _publicationRepository.CreatePublication(publication);
        return MapToDto(created);
    }

    public GetPublicationDto? UpdatePublication(int id, UpdatePublicationDto dto)
    {
        var existing = _publicationRepository.GetPublicationById(id);
        if (existing == null) return null;

        var updated = new Publication
        {
            ImageUrl = dto.ImageUrl,
            Content = dto.Content,
            Likes = dto.Likes,
            Shares = dto.Shares,
            PublicationCampaigns = dto.CampaignIds
                .Select(cid => new PublicationCampaign { CampaignId = cid })
                .ToList()
        };

        var success = _publicationRepository.UpdatePublication(id, updated);
        return success ? GetPublicationById(id) : null;
    }

    public bool DeletePublication(int id) =>
        _publicationRepository.DeletePublication(id);

    public bool AddLike(int id) =>
        _publicationRepository.AddLike(id);

    public bool AddShare(int id) =>
        _publicationRepository.AddShare(id);

    public GetCommentDto? AddComment(int publicationId, CreateCommentDto dto)
    {
        var publication = _publicationRepository.GetPublicationById(publicationId);
        if (publication == null) return null;

        var comment = new Comment { UserId = dto.UserId, Content = dto.Content };
        var success = _publicationRepository.AddComment(publicationId, comment);
        if (!success) return null;

        var user = _userRepository.GetUserById(dto.UserId);
        return new GetCommentDto(comment.Id, dto.UserId, user?.Name ?? "Unknown", dto.Content);
    }

    public GetCommentDto? UpdateComment(int publicationId, int commentId, UpdateCommentDto dto)
    {
        var publication = _publicationRepository.GetPublicationById(publicationId);
        if (publication == null) return null;

        var comment = publication.Comments.FirstOrDefault(c => c.Id == commentId);
        if (comment == null) return null;

        var success = _publicationRepository.UpdateComment(publicationId, commentId, dto.Content);
        if (!success) return null;

        var user = _userRepository.GetUserById(comment.UserId);
        return new GetCommentDto(commentId, comment.UserId, user?.Name ?? "Unknown", dto.Content);
    }

    public bool DeleteComment(int publicationId, int commentId) =>
        _publicationRepository.DeleteComment(publicationId, commentId);
}
