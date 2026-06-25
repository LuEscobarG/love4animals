using Love4AnimalsApi.Data;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Love4AnimalsApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<User> GetUsers() =>
        _context.Users.ToList();

    public User? GetUserById(int id) =>
        _context.Users.FirstOrDefault(u => u.Id == id);

    public User? GetUserByEmail(string email) =>
        _context.Users.FirstOrDefault(u => u.Email == email);

    public User CreateUser(User user)
    {
        try
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Error al crear el usuario. Verifica que los datos sean válidos.", ex);
        }
    }

    public bool UpdateUser(int id, User user)
    {
        var existing = _context.Users.FirstOrDefault(u => u.Id == id);
        if (existing == null) return false;

        existing.Name = user.Name;
        existing.Email = user.Email;
        existing.UserType = user.UserType;
        existing.Phone = user.Phone;
        existing.Bio = user.Bio;

        _context.SaveChanges();
        return true;
    }

    public bool DeleteUser(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return false;

        _context.Users.Remove(user);
        _context.SaveChanges();
        return true;
    }
}
