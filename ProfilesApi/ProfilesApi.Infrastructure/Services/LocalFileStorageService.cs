using Microsoft.Extensions.Configuration;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _currentDirectory;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _currentDirectory = configuration["FileStorageSettings:StoragePath"] ?? "wwwroot/photos";

        if (!Directory.Exists(_currentDirectory))
        {
            Directory.CreateDirectory(_currentDirectory);
        }
    }
    
    public async Task<string> UploadPhotoAsync(Stream fileStream, string fileName, CancellationToken ct = default)
    {
        var extension = Path.GetExtension(fileName);
        
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(_currentDirectory, uniqueFileName);
        
        await using var destinationStream = File.Create(fullPath);
        await fileStream.CopyToAsync(destinationStream, ct);

        return $"/photos/{uniqueFileName}";
    }

    public void DeletePhoto(string fileUrl, CancellationToken ct = default)
    {
        var fileName = Path.GetFileName(fileUrl);
        var fullPath = Path.Combine(_currentDirectory, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}