using Application.Auth.Abstractions;
using Azure.Core;
using Domain.Users.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Presentation.Auth.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly SymmetricSecurityKey _securityKey;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public TokenService(IConfiguration configuration)
        {
            var ValidIssuer = configuration.GetSection("Jwt:ValidIssuer");
            var ValidAudience = configuration.GetSection("Jwt:ValidAudience");
            var Secret = configuration.GetSection("Jwt:Secret");

            if (ValidIssuer.Value is null || ValidAudience.Value is null || Secret.Value is null)
                throw new ArgumentException("Variables for a Jwt usage are set incorrectly.");

            _issuer = ValidIssuer.Value;
            _audience = ValidAudience.Value;
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret.Value));
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>()
            {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (ClaimTypes.Name, user.Username)
            };
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken
            (
                issuer: _issuer,
                audience: _audience,
                claims,
                null,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256)
            );

            return _tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken(User user)
        {
            var claims = new List<Claim>()
            {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var token = new JwtSecurityToken
            (
                issuer: _issuer,
                audience: _audience,
                claims,
                null,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256)
            );

            return _tokenHandler.WriteToken(token);
        }

        public int? GetUserId(string token)
        {
            if (!TryParseToken(token, out var claimsPrincipal) || claimsPrincipal is null || !int.TryParse(claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Sub), out var userId))
                return null;

            return userId;
        }

        public IEnumerable<string>? GetRoles(string token)
        {
            if (!TryParseToken(token, out var claimsPrincipal) || claimsPrincipal is null)
                return null;

            var roleClaims = claimsPrincipal.FindAll(ClaimTypes.Role);
            if (!roleClaims.Any())
                return null;

            return roleClaims.Select(claim => claim.Value);
        }

        private bool TryParseToken(string token, out ClaimsPrincipal? claimsPrincipal)
        {
            claimsPrincipal = null;

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = _securityKey,
                    ValidateLifetime = true
                };

                claimsPrincipal = _tokenHandler.ValidateToken(token, validationParameters, out _);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
