using Microsoft.AspNetCore.Mvc;
using ProfilesApi.Application.Dto.Patients;
using ProfilesApi.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ProfilesApi.API.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;
    
    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }
    
    [HttpPost]
    [SwaggerOperation(
        Summary = "Adds a new patient",
        Description = "Registers a new patient with the specified details. Requires the acting user's ID in the request header",
        OperationId = "CreatePatient"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Patient was created successfully", typeof(PatientDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request body or parameters")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Email or phone number is already in use by another account")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> CreatePatient([FromBody] RegisterPatientDto registerPatientDto,
        [FromHeader(Name = "X-User-Id")] Guid createdById, CancellationToken ct = default)
    {
        var result = await _patientService.CreatePatientAsync(registerPatientDto, createdById, ct);
        return Created($"/patients/{result.Id}", result);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Deletes a patient",
        Description = "Permanently removes a patient account by its unique identifier.",
        OperationId = "DeletePatient"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Patient was successfully deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Patient with specified ID was not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> DeletePatient([FromRoute] Guid id, CancellationToken ct = default)
    {
        await _patientService.DeletePatientAsync(id, ct);
        return NoContent();
    }
    
    [HttpPut]
    [SwaggerOperation(
        Summary = "Edits a patient profile",
        Description = "Edits patient specified details. Requires the acting user's ID in the request header",
        OperationId = "EditPatient"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Patient profile was successfully edited", typeof(PatientDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request body or parameters")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Patient with specified ID was not found")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Email or phone number is already in use by another account")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> EditPatientProfile([FromBody] EditPatientProfileDto editPatientProfileDto,
        [FromHeader(Name = "X-User-Id")] Guid editedById, CancellationToken ct = default)
    {
        var editedPatient = await _patientService.EditPatientAsync(editPatientProfileDto, editedById, ct);
        return Ok(editedPatient);
    }

    [HttpGet("{patientId:guid}")]
    [SwaggerOperation(
        Summary = "Gets a patient by ID",
        Description = "Retrieves detailed information for a specific patient using their unique identifier.",
        OperationId = "GetPatientById"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Patient retrieved successfully", typeof(PatientDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Patient with specified ID was not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetPatient([FromRoute] Guid patientId, CancellationToken ct = default)
    {
        var patient = await _patientService.GetPatientAsync(patientId, ct);
        return Ok(patient);
    }
    
    [HttpGet("accounts/{accountId:guid}")]
    [SwaggerOperation(
        Summary = "Gets a patient by account ID",
        Description = "Retrieves patient details associated with a specific user account ID.",
        OperationId = "GetPatientByAccountId"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Patient retrieved successfully", typeof(PatientDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Patient with specified account ID was not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetByAccountId([FromRoute] Guid accountId, CancellationToken ct = default)
    {
        var patient = await _patientService.GetByAccountIdAsync(accountId, ct);
        return Ok(patient);
    }

    [HttpPost("search")]
    [SwaggerOperation(
        Summary = "Gets a list of patients",
        Description = "Retrieves a paginated and filtered list of patients based on search parameters.",
        OperationId = "GetPatients"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of patients retrieved successfully", typeof(IEnumerable<PatientDto>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid search or filter parameters")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetPatients(
        [FromBody] SearchFilteredPatientListDto filteredPatientListDto, CancellationToken ct = default)
    {
        var patients = await _patientService.GetPatientsAsync(filteredPatientListDto, ct);
        return Ok(patients);
    }
}