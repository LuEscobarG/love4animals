namespace Love4AnimalsApi.Models;

public class Comment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;

    // FK a Publication
    public int PublicationId { get; set; }
    public Publication Publication { get; set; } = null!;
}
