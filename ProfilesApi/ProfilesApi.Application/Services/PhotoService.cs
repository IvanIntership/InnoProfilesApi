using AutoMapper;
using ProfilesApi.Application.Dto.Photos;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Services;

public class PhotoService : IPhotoService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    
    public PhotoService(IMapper mapper, IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
    }

    public async Task<PhotoDto> UploadPhotoAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default)
    {
        var photoUrl = await _fileStorageService.UploadPhotoAsync(fileStream, fileName, ct);
        
        var photo = new Photo(photoUrl);

        _unitOfWork.Photos.Add(photo);
        await _unitOfWork.CompleteAsync(ct);

        return new PhotoDto(photo.Id, photoUrl);
    }

    public async Task DeletePhotoAsync(Guid photoId, CancellationToken ct = default)
    {
        var photo = await _unitOfWork.Photos.GetByIdAsync(photoId, ct);
        if (photo == null)
        {
            throw new KeyNotFoundException($"Photo with ID '{photoId}' was not found.");
        }

        if (!string.IsNullOrWhiteSpace(photo.Url))
        {
            _fileStorageService.DeletePhoto(photo.Url, ct);
        }

        _unitOfWork.Photos.Delete(photo);
        await _unitOfWork.CompleteAsync(ct);
    }
    
    public async Task<(Stream Stream, string ContentType)> GetPhotoAsync(Guid photoId, CancellationToken ct = default)
    {
        var photo = await _unitOfWork.Photos.GetByIdAsync(photoId, ct);
        if (photo == null)
        {
            throw new KeyNotFoundException($"Photo with ID '{photoId}' was not found.");
        }

        var fileName = Path.GetFileName(photo.Url);
        
        if (string.IsNullOrWhiteSpace(fileName)) throw new KeyNotFoundException($"Photo with ID '{photoId}' was not found.");

        var fileResult = await _fileStorageService.GetPhotoAsync(fileName, ct);
        if (fileResult == null)
        {
            throw new KeyNotFoundException($"Physical file for photo ID '{photoId}' was not found on storage.");
        }

        return fileResult.Value;
    }
}