using System.Security.Claims;
using Mero_Doctor_Project.DTOs.Admin; // Make sure this is the correct namespace for DoctorInfoDto
using Mero_Doctor_Project.Helper;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models.Enums;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly NotificationHelper _notificationHelper;

        public AdminController(IAdminRepository adminRepository, NotificationHelper notificationHelper)
        {
            _adminRepository = adminRepository;
            _notificationHelper = notificationHelper;
        }

        [HttpPut("verify/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ResponseModel<UpdateDoctorInfoDto>>> VerifyDoctor(int id, [FromBody] DoctorStatus status)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var errorMessage = "Invalid Credentials: " + string.Join("; ", errors);

                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = errorMessage,
                    Data = null
                });
            }

            var result = await _adminRepository.VerifyDoctorAsync(id, status);

            if (result.Success)
            {
                var doctorUserId = result.Data.UserId;

                var message = $"Your verification status has been updated to {status}.";

                await _notificationHelper.NotifyUserAsync(doctorUserId, message);

                return Ok(result);
            }

            return NotFound(result);
        }


        [HttpGet("verified")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]

        public async Task<ActionResult<ResponseModel<List<GetDoctorInfoDto>>>> GetVerifiedDoctors()
        {
            var result = await _adminRepository.GetVerifiedDoctorsAsync();
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet("pending")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]

        public async Task<ActionResult<ResponseModel<List<GetDoctorInfoDto>>>> GetPendingDoctors()
        {
            var result = await _adminRepository.GetPendingDoctorsAsync();
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet("rejected")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]

        public async Task<ActionResult<ResponseModel<List<GetDoctorInfoDto>>>> GetRejectedDoctors()
        {
            var result = await _adminRepository.GetRejectedDoctorsAsync();
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
        [HttpGet("dashboard")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ResponseModel<AdminDashboardViewDto>>> DashboardView()
        {
            var result = await _adminRepository.DashboardView();
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet("profile")]

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ResponseModel<GetAdminDto>>> AdminProfile()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _adminRepository.GetAdminById(userId);
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
        }

    }
