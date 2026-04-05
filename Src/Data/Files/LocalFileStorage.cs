using Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Transversal.Configurations;

namespace Data.Files;

public class LocalFileStorage : IFileStorage
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly FileStorageOptions _fileStorageOptions;
    public LocalFileStorage(IWebHostEnvironment webHostEnvironment, IOptions<FileStorageOptions> fileStorageOptions)
    {
        _webHostEnvironment = webHostEnvironment;
        _fileStorageOptions = fileStorageOptions.Value;
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
        Console.WriteLine($"ContentRootPath: {_webHostEnvironment.ContentRootPath}");
        Console.WriteLine($"StoragePath resuelto: {storagePath}");
        return nameFile;
    }
}
