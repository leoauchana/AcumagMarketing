using Domain.Common;

namespace Domain.Entities;

public class User : EntityBase
{
    public string Username { get; private set; }
    public string Password { get; private set; }
    public Employee Employee { get; private set; }

    public User()
    {
    }

    public User(string userName, string password)
    {
        Username = userName;
        Password = password;
    }
}