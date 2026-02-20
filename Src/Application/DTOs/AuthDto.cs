namespace Application.DTOs;

public class AuthDto
{
    public record Login(string userName, string password);
    public record Response(string id, string fisrtName, string lastName, string role, string token);
}