using Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Transversal.Configurations;

namespace Data.Files;

public class LocalFileStorage : IFileStorage
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly FileStorageOptions _fileStorageOptions;
    public LocalFileStorage(IWebHostEnvironment webHostEnvironment, FileStorageOptions fileStorageOptions)
    {
        _webHostEnvironment = webHostEnvironment;
        _fileStorageOptions = fileStorageOptions;
    }

    public async Task<string> SaveFile(Stream file, string name, string contentType)
    {
        var pathRelative = _fileStorageOptions.StoragePath;
        var storagePath = Path.GetFullPath(Path.Combine(_webHostEnvironment.ContentRootPath, pathRelative));
        if(!Directory.Exists(storagePath)) Directory.CreateDirectory(storagePath);
        var nameFile = $"{Guid.NewGuid()}{Path.GetExtension(name)}";
        var pathComplete = Path.Combine(storagePath, nameFile);
        using var fileStream = new FileStream(pathComplete, FileMode.Create);
        await file.CopyToAsync(fileStream);
        return nameFile;
    }
}
