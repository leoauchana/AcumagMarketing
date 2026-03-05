namespace Application.DTOs;

public class EmployeeDto
{
    public record Request(string firstName, string lastName, string email, string dni, string city, string street, int number, string zipCode, string idRole
        , string userName, string password);
    public record RequestUpdate(string id, string idRole);
    public record Response(string id, string fisrtName, string lastName, string email, string dni, string city, string street, int number, string role);
    public record ReponseWithQuote(string id, string firstName, string lastName);
}