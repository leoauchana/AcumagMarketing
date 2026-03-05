namespace Domain.Interfaces;

public interface IFileStorage
{
    Task<string> SaveFile(Stream file, string name, string contentType);
}
