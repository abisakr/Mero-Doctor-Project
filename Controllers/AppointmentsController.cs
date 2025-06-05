using Mero_Doctor_Project.DTOs.AppointmentDto;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Repositories.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentsController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        [HttpPost("bookAppointment")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentDto dto)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Unauthorized: user ID not found",
                    Data = null
                });
            }

            var result = await _appointmentRepository.BookAppointmentAsync(dto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-status")]
        //[Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateAppointmentStatus([FromBody] UpdateAppointmentStatusDto dto)
        {
            string doctorUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (doctorUserId == null)
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Unauthorized: Doctor user ID not found.",
                    Data = null
                });

            var result = await _appointmentRepository.UpdateAppointmentStatusAsync(dto, doctorUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("doctor/{doctorId}")]
        //[Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAppointmentsByDoctor(int doctorId)
        {
            var result = await _appointmentRepository.GetAppointmentsByDoctorAsync(doctorId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Patient: Get all appointments
        [HttpGet("patient")]
      [Authorize(AuthenticationSchemes = "Bearer", Roles = "Patient")]
        public async Task<IActionResult> GetAppointmentsByPatient()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Unauthorized: Patient ID not found",
                    Data = null
                });
            }

            var result = await _appointmentRepository.GetAppointmentsByPatientAsync(userId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
