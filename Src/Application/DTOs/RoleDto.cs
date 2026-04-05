namespace Application.DTOs;

public class RoleDto
{
    public record Request(string rolName, string description);
    public record Response(string id, string rolName, string description);
}
