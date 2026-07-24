using Microsoft.AspNetCore.Mvc;
using ProfilesApi.Application.Dto.Administrators;
using ProfilesApi.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ProfilesApi.API.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
public class AdministratorsController : ControllerBase
{
    private readonly IAdministratorService _administratorService;
    
    public AdministratorsController(IAdministratorService administratorService)
    {
        _administratorService = administratorService;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Adds a new administrator",
        Description = "Registers a new system administrator with the specified details. Requires the acting user's ID in the request header",
        OperationId = "AddAdministrator"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Administrator was created successfully", typeof(AdministratorDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> CreateAdministrator(CreateAdministratorDto createAdministratorDto,
        [FromHeader(Name = "X-User-Id")] Guid createdById, CancellationToken ct = default)
    {
        var result = await _administratorService.CreateAdministratorAsync(createAdministratorDto, createdById, ct);
        return Created($"/administrators/{result.Id}", result);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Deletes an administrator",
        Description = "Permanently removes a system administrator account by its unique identifier.",
        OperationId = "DeleteAdministrator"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Administrator was successfully deleted")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> DeleteAdministrator(Guid id, CancellationToken ct = default)
    {
        await _administratorService.DeleteAdministratorAsync(id, ct);
        return NoContent();
    }
    
    [HttpPut]
    [SwaggerOperation(
        Summary = "Edits an administrator profile",
        Description = "Edits system administrators specified details. Requires the acting user's ID in the request header",
        OperationId = "EditAdministrator"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Administrator was successfully edited", typeof(AdministratorDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> EditAdministratorProfile(EditAdministratorProfileDto editAdministratorProfileDto,
        [FromHeader(Name = "X-User-Id")] Guid editedById, CancellationToken ct = default)
    {
        var editedAdministrator = await _administratorService.EditAdministratorProfileAsync(editAdministratorProfileDto, editedById, ct);
        return Ok(editedAdministrator);
    }

    [HttpGet("{administratorId:guid}")]
    [SwaggerOperation(
        Summary = "Gets an administrator by ID",
        Description = "Retrieves detailed information for a specific administrator using their unique identifier.",
        OperationId = "GetAdministratorById"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Administrator retrieved successfully", typeof(AdministratorDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetAdministrator(Guid administratorId, CancellationToken ct = default)
    {
        var administrator = await _administratorService.GetAdministratorAsync(administratorId, ct);
        return Ok(administrator);
    }
    
    [HttpGet("accounts/{accountId:guid}")]
    [SwaggerOperation(
        Summary = "Gets an administrator by account ID",
        Description = "Retrieves administrator details associated with a specific user account ID.",
        OperationId = "GetAdministratorByAccountId"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Administrator retrieved successfully", typeof(AdministratorDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetByAccountId(Guid accountId, CancellationToken ct = default)
    {
        var administrator = await _administratorService.GetByAccountIdAsync(accountId, ct);
        return Ok(administrator);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets a list of administrators",
        Description = "Retrieves a paginated and filtered list of administrators based on search parameters.",
        OperationId = "GetAdministrators"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of administrators retrieved successfully", typeof(IEnumerable<AdministratorDto>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetAdministrators(
        [FromQuery] SearchFilteredAdministratorListDto filteredAdministratorListDto, CancellationToken ct = default)
    {
        var administrators = await _administratorService.GetAdministratorsAsync(filteredAdministratorListDto, ct);
        return Ok(administrators);
    }
}