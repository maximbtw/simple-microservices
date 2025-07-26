using Auth.Contracts.Persistence.User;

namespace Auth.Contracts;

public interface ITokenProvider
{
    string GenerateToken(UserDto user);

    bool TryGetUserIdFromToken(string token, out int userId);
}