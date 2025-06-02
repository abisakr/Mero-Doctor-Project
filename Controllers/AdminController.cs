using Mero_Doctor_Project.DTOs.Admin; // Make sure this is the correct namespace for DoctorInfoDto
using Mero_Doctor_Project.Helper;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models.Enums;
using Mero_Doctor_Project.Repositories.Interfaces;
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
        public async Task<ActionResult<ResponseModel<DoctorInfoDto>>> VerifyDoctor(int id, [FromBody] DoctorStatus status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _adminRepository.VerifyDoctorAsync(id, status);
            if (result.Success)
            {
                await _notificationHelper.NotifyAdminsAsync($"A doctor has {status}  successfully.");
                return Ok(result);

            }

            return NotFound(result);
        }

        [HttpGet("verified")]
        public async Task<ActionResult<ResponseModel<List<DoctorInfoDto>>>> GetVerifiedDoctors()
        {
            var result = await _adminRepository.GetVerifiedDoctorsAsync();
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet("pending")]
        public async Task<ActionResult<ResponseModel<List<DoctorInfoDto>>>> GetPendingDoctors()
        {
            var result = await _adminRepository.GetPendingDoctorsAsync();
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet("rejected")]
        public async Task<ActionResult<ResponseModel<List<DoctorInfoDto>>>> GetRejectedDoctors()
        {
            var result = await _adminRepository.GetRejectedDoctorsAsync();
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
    }
}
