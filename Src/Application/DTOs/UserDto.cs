namespace Application.DTOs;

public class UserDto
{
    public record Request(string idUser, string idNewRole);

    public record Response();
}