namespace ProfilesApi.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadPhotoAsync(Stream fileStream, string fileName, CancellationToken ct = default);
    void DeletePhoto(string fileUrl, CancellationToken ct = default);
    Task<(Stream Stream, string ContentType)?> GetPhotoAsync(string fileName, CancellationToken ct = default);
}