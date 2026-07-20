using System.Security.Claims;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientsController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Patient,Doctor")]
        [HttpGet("getPatientById")]
        public async Task<IActionResult> GetPatientById([FromQuery] int? patientId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized("User session invalid.");

            var response = await _patientRepository.GetPatientDetailsAsync(patientId, currentUserId);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("getAllPatients")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientRepository.GetAllPatientsAsync();

            if (patients.Data.Count == 0)
            {
                return NotFound(patients);
            }

            return Ok(patients);
        }
    }
}