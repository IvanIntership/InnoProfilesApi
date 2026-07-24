using Microsoft.AspNetCore.Mvc;
using ProfilesApi.Application.Dto.Offices;
using ProfilesApi.Application.Dto.Shared;
using ProfilesApi.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ProfilesApi.API.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
public class OfficesController : ControllerBase
{
    private readonly IOfficeService _officeService;
    
    public OfficesController(IOfficeService officeService)
    {
        _officeService = officeService;
    }
    
    [HttpPost]
    [SwaggerOperation(
        Summary = "Adds a new office",
        Description = "Registers a new office with the specified details.",
        OperationId = "CreateOffice"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Office was created successfully", typeof(OfficeDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> CreateOffice(CreateOfficeDto createOfficeDto, CancellationToken ct = default)
    {
        var result = await _officeService.CreateOfficeAsync(createOfficeDto, ct);
        return Created($"/offices/{result.Id}", result);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Deletes an office",
        Description = "Permanently removes an office by its unique identifier.",
        OperationId = "DeleteOffice"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Office was successfully deleted")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> DeleteOffice(Guid id, CancellationToken ct = default)
    {
        await _officeService.DeleteOfficeAsync(id, ct);
        return NoContent();
    }
    
    [HttpPut]
    [SwaggerOperation(
        Summary = "Edits office information",
        Description = "Edits specified office details.",
        OperationId = "EditOffice"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Office was successfully edited")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> EditOffice(EditOfficeInformationDto editOfficeInformationDto, CancellationToken ct = default)
    {
        await _officeService.EditOfficeAsync(editOfficeInformationDto, ct);
        return NoContent();
    }

    [HttpGet("{officeId:guid}")]
    [SwaggerOperation(
        Summary = "Gets an office by ID",
        Description = "Retrieves detailed information for a specific office using its unique identifier.",
        OperationId = "GetOfficeById"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Office retrieved successfully", typeof(OfficeDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetOffice(Guid officeId, CancellationToken ct = default)
    {
        var office = await _officeService.GetOfficeByIdAsync(officeId, ct);
        return Ok(office);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets a list of offices",
        Description = "Retrieves a paginated and filtered list of offices based on search parameters.",
        OperationId = "GetOffices"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of offices retrieved successfully", typeof(IEnumerable<OfficeDto>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetOffices(
        [FromQuery] SearchQueryDto filteredOfficeListDto, CancellationToken ct = default)
    {
        var offices = await _officeService.GetOfficeListAsync(filteredOfficeListDto, ct);
        return Ok(offices);
    }
}