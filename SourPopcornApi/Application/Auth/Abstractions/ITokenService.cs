using Domain.Users.Entities;

namespace Application.Auth.Abstractions;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    int? GetUserId(string token);
    IEnumerable<string>? GetRoles(string token);
}
