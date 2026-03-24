using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Interfaces;

public interface IUserRepository
{
    List<User> GetUsers();
    User? GetUserById(int id);
    User CreateUser(User user);
    bool UpdateUser(int id, User user);
    bool DeleteUser(int id);
}