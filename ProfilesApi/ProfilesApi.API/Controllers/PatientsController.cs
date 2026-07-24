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
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> CreatePatient(RegisterPatientDto registerPatientDto,
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
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> DeletePatient(Guid id, CancellationToken ct = default)
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
    [SwaggerResponse(StatusCodes.Status200OK, "Patient was successfully edited", typeof(PatientDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> EditPatientProfile(EditPatientProfileDto editPatientProfileDto,
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
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetPatient(Guid patientId, CancellationToken ct = default)
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
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetByAccountId(Guid accountId, CancellationToken ct = default)
    {
        var patient = await _patientService.GetByAccountIdAsync(accountId, ct);
        return Ok(patient);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets a list of patients",
        Description = "Retrieves a paginated and filtered list of patients based on search parameters.",
        OperationId = "GetPatients"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of patients retrieved successfully", typeof(IEnumerable<PatientDto>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal service error")]
    public async Task<IActionResult> GetPatients(
        [FromQuery] SearchFilteredPatientListDto filteredPatientListDto, CancellationToken ct = default)
    {
        var patients = await _patientService.GetPatientsAsync(filteredPatientListDto, ct);
        return Ok(patients);
    }
}