namespace Love4AnimalsApi.Models;

public class Comment
{
    public Comment(int id, int userId, string content)
    {
        Id = id;
        UserId = userId;
        Content = content;
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
}
