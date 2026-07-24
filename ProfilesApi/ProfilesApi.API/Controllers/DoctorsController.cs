using Microsoft.AspNetCore.Mvc;
using ProfilesApi.Application.Dto.Doctors;
using ProfilesApi.Application.Dto.Doctors;
using ProfilesApi.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ProfilesApi.API.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }
    
    [HttpPost]
    [SwaggerOperation(
        Summary = "Adds a new doctor",
        Description = "Registers a new system doctor with the specified details. Requires the acting user's ID in the request header",
        OperationId = "AddDoctor"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Doctor was created successfully", typeof(DoctorDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDto createDoctorDto,
        [FromHeader(Name = "X-User-Id")] Guid createdById, CancellationToken ct = default)
    {
        var result = await _doctorService.CreateDoctorAsync(createDoctorDto, createdById, ct);
        return Created($"/doctors/{result.Id}", result);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Deletes an doctor",
        Description = "Permanently removes a system doctor account by its unique identifier.",
        OperationId = "DeleteDoctor"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Doctor was successfully deleted")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> DeleteDoctor([FromRoute] Guid id, CancellationToken ct = default)
    {
        await _doctorService.DeleteDoctorAsync(id, ct);
        return NoContent();
    }
    
    [HttpPut]
    [SwaggerOperation(
        Summary = "Edits an doctor profile",
        Description = "Edits system doctor specified details. Requires the acting user's ID in the request header",
        OperationId = "EditDoctor"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor was successfully edited", typeof(DoctorDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> EditDoctorProfile([FromBody] EditDoctorProfileDto editDoctorProfileDto,
        [FromHeader(Name = "X-User-Id")] Guid editedById, CancellationToken ct = default)
    {
        var editedDoctor = await _doctorService.EditDoctorProfileAsync(editDoctorProfileDto, editedById, ct);
        return Ok(editedDoctor);
    }

    [HttpGet("{doctorId:guid}")]
    [SwaggerOperation(
        Summary = "Gets an doctor by ID",
        Description = "Retrieves detailed information for a specific doctor using their unique identifier.",
        OperationId = "GetDoctorById"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor retrieved successfully", typeof(DoctorDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetDoctor([FromRoute] Guid doctorId, CancellationToken ct = default)
    {
        var doctor = await _doctorService.GetDoctorAsync(doctorId, ct);
        return Ok(doctor);
    }
    
    [HttpGet("accounts/{accountId:guid}")]
    [SwaggerOperation(
        Summary = "Gets an doctor by account ID",
        Description = "Retrieves doctor details associated with a specific user account ID.",
        OperationId = "GetDoctorByAccountId"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor retrieved successfully", typeof(DoctorDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetByAccountId([FromRoute] Guid accountId, CancellationToken ct = default)
    {
        var doctor = await _doctorService.GetByAccountIdAsync(accountId, ct);
        return Ok(doctor);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Gets a list of doctors",
        Description = "Retrieves a paginated and filtered list of doctors based on search parameters.",
        OperationId = "GetDoctors"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of Doctors retrieved successfully", typeof(IEnumerable<DoctorDto>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetDoctors(
        [FromBody] SearchFilteredDoctorListDto filteredDoctorListDto, CancellationToken ct = default)
    {
        var doctors = await _doctorService.GetDoctorsAsync(filteredDoctorListDto, ct);
        return Ok(doctors);
    }
}