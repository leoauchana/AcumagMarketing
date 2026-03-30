using Domain.Entities;

namespace Transversal.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}