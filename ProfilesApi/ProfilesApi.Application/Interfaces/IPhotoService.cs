using ProfilesApi.Application.Dto.Photos;

namespace ProfilesApi.Application.Interfaces;

public interface IPhotoService
{
    Task<PhotoDto> UploadPhotoAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default);
    Task DeletePhotoAsync(Guid photoId, CancellationToken ct = default);
    Task<(Stream FileStream, string ContentType)?> GetPhotoFileAsync(Guid photoId, CancellationToken ct = default);
}