using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Services;

public class UserService : IUserService
{
    private IUserRepository userRepository;

    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public List<GetUserDto> GetUsers()
    {
        return userRepository.GetUsers()
            .Select(user => new GetUserDto(user.Id, user.Name, user.Email))
            .ToList();
    }

    public GetUserDto? GetUserById(int id)
    {
        var user = userRepository.GetUserById(id);

        if (user == null)
            return null;

        return new GetUserDto(user.Id, user.Name, user.Email);
    }

    public GetUserDto CreateUser(CreateUserDto dto)
    {
        var users = userRepository.GetUsers();
        int newId = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;

        User newUser = new User(newId, dto.Name, dto.Email);
        var createdUser = userRepository.CreateUser(newUser);

        return new GetUserDto(createdUser.Id, createdUser.Name, createdUser.Email);
    }

    public GetUserDto? UpdateUser(int id, UpdateUserDto dto)
    {
        User updatedUser = new User(id, dto.Name, dto.Email);
        var success = userRepository.UpdateUser(id, updatedUser);

        if (!success)
            return null;

        return new GetUserDto(id, dto.Name, dto.Email);
    }

    public bool DeleteUser(int id)
    {
        return userRepository.DeleteUser(id);
    }
}