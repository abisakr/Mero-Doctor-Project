using Mero_Doctor_Project.DTOs.AuthDto;
using Mero_Doctor_Project.Models.Enums;
using Mero_Doctor_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mero_Doctor_Project.Repositories.Interfaces;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Repositories;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthDoctorRegistrationController : ControllerBase
    {
        private readonly IAuthDoctorRegistrationRepository _authDoctorRegistrationRepository;

        public AuthDoctorRegistrationController(IAuthDoctorRegistrationRepository authDoctorRegistrationRepository)
        {
            _authDoctorRegistrationRepository = authDoctorRegistrationRepository;
        }

        [HttpPost("doctorLogin")]
        public async Task<ActionResult<ResponseModel<string>>> DoctorLoginAsync([FromBody] DoctorLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authDoctorRegistrationRepository.DoctorLoginAsync(loginDto);

            if (result.Success)
                return Ok(result);

            return Unauthorized(result);
        }

        [HttpPost("register-doctor")]
        public async Task<IActionResult> Register([FromBody] DoctorRegistrationDto dto)
        {
            // Check if the ModelState is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            // Call the registration method
            var response = await _authDoctorRegistrationRepository.DoctorRegisterAsync(dto);

            // Check the result from the service
            if (response.Success)
            {
                return Ok(response); // Return success 
            }
            else
            {
                return BadRequest(response); // Return failure with the message
            }
        }



    }
}
