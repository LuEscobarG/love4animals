using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Repositories;

public class PublicationRepository : IPublicationRepository
{
    private List<Publication> Publications { get; set; }

    public PublicationRepository()
    {
        Publications = new List<Publication>();

        Publications.Add(new Publication(1, "Ayuda a los animales silvestres de nuestra región", new List<int> { 1 }));
        Publications.Add(new Publication(2, "Campaña de alimento para el refugio local", new List<int> { 2 }));
    }

    public List<Publication> GetPublications()
    {
        return Publications;
    }

    public Publication? GetPublicationById(int id)
    {
        return Publications.FirstOrDefault(p => p.Id == id);
    }

    public Publication CreatePublication(Publication publication)
    {
        Publications.Add(publication);
        return publication;
    }

    public bool UpdatePublication(int id, Publication publication)
    {
        var existing = Publications.FirstOrDefault(p => p.Id == id);

        if (existing == null)
            return false;

        existing.Content = publication.Content;
        existing.CampaignIds = publication.CampaignIds;
        existing.Likes = publication.Likes;
        existing.Shares = publication.Shares;

        return true;
    }

    public bool DeletePublication(int id)
    {
        var publication = Publications.FirstOrDefault(p => p.Id == id);

        if (publication == null)
            return false;

        Publications.Remove(publication);
        return true;
    }

    public bool AddLike(int id)
    {
        var publication = Publications.FirstOrDefault(p => p.Id == id);

        if (publication == null)
            return false;

        publication.Likes++;
        return true;
    }

    public bool AddShare(int id)
    {
        var publication = Publications.FirstOrDefault(p => p.Id == id);

        if (publication == null)
            return false;

        publication.Shares++;
        return true;
    }

    public bool AddComment(int id, Comment comment)
    {
        var publication = Publications.FirstOrDefault(p => p.Id == id);

        if (publication == null)
            return false;

        publication.Comments.Add(comment);
        return true;
    }
}
