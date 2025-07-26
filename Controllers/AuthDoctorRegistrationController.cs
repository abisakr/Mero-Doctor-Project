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
using Mero_Doctor_Project.Helper;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthDoctorRegistrationController : ControllerBase
    {
        private readonly IAuthDoctorRegistrationRepository _authDoctorRegistrationRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly NotificationHelper _notificationHelper;
        public AuthDoctorRegistrationController(UserManager<ApplicationUser> userManager ,IAuthDoctorRegistrationRepository authDoctorRegistrationRepository, NotificationHelper notificationHelper)
        {
            _userManager = userManager;
            _authDoctorRegistrationRepository = authDoctorRegistrationRepository;
            _notificationHelper = notificationHelper;

        }

        [HttpPost("doctorLogin")]
        public async Task<ActionResult<ResponseModel<string>>> DoctorLoginAsync([FromBody] DoctorLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                var errorMessage = $"Invalid Credentials: {firstError}";

                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = errorMessage,
                    Data = null
                });
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
                var firstError = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                var errorMessage = $"Invalid Credentials: {firstError}";

                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = errorMessage,
                    Data = null
                });
            }

            // Call the registration method
            var response = await _authDoctorRegistrationRepository.DoctorRegisterAsync(dto);

            // Check the result from the service
            if (response.Success)
            {
                string message = $"New Doctor Registered:{dto.FullName}";
                var admins = await _userManager.GetUsersInRoleAsync("Admin");
                var tasks = admins.Select(admin =>
             _notificationHelper.SendAndStoreNotificationAsync(admin.Id, message));
                await Task.WhenAll(tasks);
                return Ok(response); // Return success 
            }
            else
            {
                return BadRequest(response); // Return failure with the message
            }
        }



    }
}
