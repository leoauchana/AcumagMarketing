using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using Transversal.Configurations;
using Transversal.Interfaces;

namespace Transversal.Security;

public class JwtTokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly ClaimsFactory _claimsFactory;
    public JwtTokenService(JwtOptions jwtOptions, ClaimsFactory claimsFactory)
    {
        _jwtOptions = jwtOptions;
        _claimsFactory  = claimsFactory;
    }
    public string GenerateToken(User user)
    {
        var key = _jwtOptions.Key ?? throw new InvalidOperationException("Jwt:Key not configured");
        var issuer = _jwtOptions.Issuer ?? string.Empty;
        var audience = _jwtOptions.Audience ?? string.Empty;
        var expiresMinutesStr = _jwtOptions.ExpiresMinutes;
        var expiresMinutes = int.TryParse(expiresMinutesStr, out var m) ? m : 60;
        var claims = _claimsFactory.CreateClaims(user);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}