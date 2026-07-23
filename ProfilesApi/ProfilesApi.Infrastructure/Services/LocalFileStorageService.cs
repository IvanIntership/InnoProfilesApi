using Microsoft.Extensions.Configuration;
using ProfilesApi.Application.Interfaces;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;

namespace ProfilesApi.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _currentDirectory;
    private readonly PhysicalFileProvider _fileProvider;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _currentDirectory = configuration["FileStorageSettings:StoragePath"] ?? "wwwroot/photos";

        if (!Directory.Exists(_currentDirectory))
        {
            Directory.CreateDirectory(_currentDirectory);
        }
        
        _fileProvider = new PhysicalFileProvider(_currentDirectory);
    }
    
    public async Task<string> UploadPhotoAsync(Stream fileStream, string fileName, CancellationToken ct = default)
    {
        var extension = Path.GetExtension(fileName);
        
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(_currentDirectory, uniqueFileName);

        using var destinationStream = File.Create(fullPath);
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

    public async Task<(Stream Stream, string ContentType)?> GetPhotoAsync(string fileName, CancellationToken ct = default)
    {
        IFileInfo fileInfo = _fileProvider.GetFileInfo(fileName);

        if (!fileInfo.Exists)
        {
            return await Task.FromResult<(Stream Stream, string ContentType)?>(null);
        }

        Stream stream = fileInfo.CreateReadStream();
        
        new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType);

        return await Task.FromResult<(Stream Stream, string ContentType)?>((stream, contentType ?? "application/octet-stream"));
    }
}