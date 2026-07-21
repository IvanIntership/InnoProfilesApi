using AutoMapper;
using ProfilesApi.Application.Dto.Photos;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Services;

public class PhotoService : IPhotoService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public PhotoService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<PhotoDto> UploadPhotoAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeletePhotoAsync(Guid photoId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<(Stream FileStream, string ContentType)?> GetPhotoFileAsync(Guid photoId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}