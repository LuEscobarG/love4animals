using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    private GetUserDto MapToDto(User u) =>
        new GetUserDto(u.Id, u.Name, u.Email, u.PasswordHash, u.UserType.ToString(), u.Phone, u.Bio);

    private UserType ParseUserType(string userType) =>
        Enum.TryParse<UserType>(userType, true, out var parsed) ? parsed : UserType.Missionary;

    public List<GetUserDto> GetUsers() =>
        _userRepository.GetUsers().Select(MapToDto).ToList();

    public GetUserDto? GetUserById(int id)
    {
        var user = _userRepository.GetUserById(id);
        return user == null ? null : MapToDto(user);
    }

    public GetUserDto CreateUser(CreateUserDto dto)
    {
        var newUser = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            UserType = ParseUserType(dto.UserType),
            Phone = dto.Phone,
            Bio = dto.Bio
        };
        var created = _userRepository.CreateUser(newUser);
        return MapToDto(created);
    }

    public GetUserDto? UpdateUser(int id, UpdateUserDto dto)
    {
        var updated = new User
        {
            Id = id,
            Name = dto.Name,
            Email = dto.Email,
            UserType = ParseUserType(dto.UserType),
            Phone = dto.Phone,
            Bio = dto.Bio
        };
        var success = _userRepository.UpdateUser(id, updated);
        return success ? MapToDto(updated) : null;
    }

    public bool DeleteUser(int id) =>
        _userRepository.DeleteUser(id);

    public LoginResponseDto? Login(LoginDto dto)
    {
        var user = _userRepository.GetUserByEmail(dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        var expiry = _jwtService.AccessTokenExpiry;

        return new LoginResponseDto(user.Id, user.Name, user.Email, user.UserType.ToString(), accessToken, refreshToken, expiry);
    }
}
