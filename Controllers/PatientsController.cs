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

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Patient")]
        [HttpGet("getPatientById")]
        public async Task<IActionResult> GetPatientById()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Please Login as Patient.");
            var response = await _patientRepository.GetPatientByIdAsync(userId);
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
