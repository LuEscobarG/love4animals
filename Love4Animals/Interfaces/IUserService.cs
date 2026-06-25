using Love4AnimalsApi.Dtos;

namespace Love4AnimalsApi.Interfaces;

public interface IUserService
{
    List<GetUserDto> GetUsers();
    GetUserDto? GetUserById(int id);
    GetUserDto CreateUser(CreateUserDto dto);
    GetUserDto? UpdateUser(int id, UpdateUserDto dto);
    bool DeleteUser(int id);
    LoginResponseDto? Login(LoginDto dto);
}