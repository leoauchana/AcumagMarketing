using System.Security.Claims;
using Domain.Entities;

namespace Transversal.Security;

public class ClaimsFactory
{
    public List<Claim> CreateClaims(User user)
    {
        return new List<Claim>()
        { 
            new Claim(ClaimTypes.Role,  user.Employee.Role.Name),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };
    }
}