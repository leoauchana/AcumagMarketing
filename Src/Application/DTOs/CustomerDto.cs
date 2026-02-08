namespace Application.DTOs;

public class CustomerDto
{
 public record Request(string firstName, string lastName, string email, string dni, string street, string city, int number
 , string phoneNumber, string zipCode);
 public record RequestUpdate(string firstName, string lastName, string email, string dni, string street, string city, int number
  , string phoneNumber, string zipCode, string id);
 public record Response();
}