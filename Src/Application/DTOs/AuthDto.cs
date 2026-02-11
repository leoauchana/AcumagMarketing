namespace Application.DTOs;

public class AuthDto
{
    public record Login(string userName, string password);
}