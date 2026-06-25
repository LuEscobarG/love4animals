using Love4AnimalsApi.Data;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Love4AnimalsApi.Repositories;

public class PublicationRepository : IPublicationRepository
{
    private readonly AppDbContext _context;

    public PublicationRepository(AppDbContext context)
    {
        _context = context;
    }

    private IQueryable<Publication> WithIncludes() =>
        _context.Publications
            .Include(p => p.Comments)
            .Include(p => p.PublicationCampaigns);

    public List<Publication> GetPublications() =>
        WithIncludes().ToList();

    public Publication? GetPublicationById(int id) =>
        WithIncludes().FirstOrDefault(p => p.Id == id);

    public Publication CreatePublication(Publication publication)
    {
        _context.Publications.Add(publication);
        _context.SaveChanges();
        return publication;
    }

    public bool UpdatePublication(int id, Publication publication)
    {
        var existing = WithIncludes().FirstOrDefault(p => p.Id == id);
        if (existing == null) return false;

        existing.ImageUrl = publication.ImageUrl;
        existing.Content = publication.Content;
        existing.Likes = publication.Likes;
        existing.Shares = publication.Shares;

        // Reemplazar relaciones de campaña
        existing.PublicationCampaigns.Clear();
        foreach (var pc in publication.PublicationCampaigns)
            existing.PublicationCampaigns.Add(new PublicationCampaign { PublicationId = id, CampaignId = pc.CampaignId });

        _context.SaveChanges();
        return true;
    }

    public bool DeletePublication(int id)
    {
        var publication = _context.Publications.FirstOrDefault(p => p.Id == id);
        if (publication == null) return false;

        _context.Publications.Remove(publication);
        _context.SaveChanges();
        return true;
    }

    public bool AddLike(int id)
    {
        var publication = _context.Publications.FirstOrDefault(p => p.Id == id);
        if (publication == null) return false;

        publication.Likes++;
        _context.SaveChanges();
        return true;
    }

    public bool AddShare(int id)
    {
        var publication = _context.Publications.FirstOrDefault(p => p.Id == id);
        if (publication == null) return false;

        publication.Shares++;
        _context.SaveChanges();
        return true;
    }

    public bool AddComment(int publicationId, Comment comment)
    {
        var publication = _context.Publications.FirstOrDefault(p => p.Id == publicationId);
        if (publication == null) return false;

        comment.PublicationId = publicationId;
        _context.Comments.Add(comment);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateComment(int publicationId, int commentId, string content)
    {
        var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId && c.PublicationId == publicationId);
        if (comment == null) return false;

        comment.Content = content;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteComment(int publicationId, int commentId)
    {
        var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId && c.PublicationId == publicationId);
        if (comment == null) return false;

        _context.Comments.Remove(comment);
        _context.SaveChanges();
        return true;
    }
}
