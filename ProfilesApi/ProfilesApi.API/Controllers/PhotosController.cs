using Microsoft.AspNetCore.Mvc;
using ProfilesApi.Application.Dto.Photos;
using ProfilesApi.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ProfilesApi.API.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
public class PhotosController : ControllerBase
{
    private readonly IPhotoService _photoService;
    
    public PhotosController(IPhotoService photoService)
    {
        _photoService = photoService;
    }

    [HttpDelete("{photoId:guid}")]
    [SwaggerOperation(
        Summary = "Deletes a photo",
        Description = "Permanently removes a photo by its unique identifier.",
        OperationId = "DeletePhoto"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Photo was successfully deleted")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> DeletePhoto(Guid photoId, CancellationToken ct = default)
    {
        await _photoService.DeletePhotoAsync(photoId, ct);
        return NoContent();
    }

    [HttpGet("{photoId:guid}")]
    [SwaggerOperation(
        Summary = "Gets a photo by ID",
        Description = "Retrieves detailed information for a specific photo using its unique identifier.",
        OperationId = "GetPhotoById"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Photo retrieved successfully", typeof(PhotoDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetPhoto(Guid photoId, CancellationToken ct = default)
    {
        var photo = await _photoService.GetPhotoAsync(photoId, ct);
        return Ok(photo);
    }
    
    [HttpPost]
    [SwaggerOperation(
        Summary = "Uploads a photo",
        Description = "Uploads a new photo file to the system.",
        OperationId = "UploadPhoto"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Photo was uploaded successfully", typeof(PhotoDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> UploadPhoto(IFormFile file, CancellationToken ct)
    { 
        using var stream = file.OpenReadStream();

        var result = await _photoService.UploadPhotoAsync(
            stream, 
            file.FileName, 
            file.ContentType, 
            ct);

        return Created($"/photos/{result.Id}", result);
    }
}