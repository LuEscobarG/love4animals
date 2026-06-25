namespace Love4AnimalsApi.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserType UserType { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
}
