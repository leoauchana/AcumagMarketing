namespace Application.DTOs;

public class EmployeeDto
{
    public record Response(string fisrtName, string lastName, string role, string token);

    public record Request();
    public record RequestUpdate();
}