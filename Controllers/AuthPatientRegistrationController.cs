using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Repositories;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthPatientRegistrationController : ControllerBase
    {
        private readonly IAuthPatientRegistrationRepository _authPatientRegistrationRepository;

        public AuthPatientRegistrationController(IAuthPatientRegistrationRepository authPatientRegistrationRepository)
        {
            _authPatientRegistrationRepository = authPatientRegistrationRepository;
        }

        [HttpPost("register-patient")]
        public async Task<IActionResult> Register([FromBody] PatientRegistrationDto dto)
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            // Call the registration method
            var response = await _authPatientRegistrationRepository.PatientRegisterAsync(dto);

            // Return appropriate response based on success or failure
            if (response.Success)
            {
                return Ok(response); // Return success with message
            }
            else
            {
                return BadRequest(response); // Return failure with message
            }
        }
    }
}
