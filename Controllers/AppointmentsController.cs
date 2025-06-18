using Mero_Doctor_Project.DTOs.AppointmentDto;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Repositories.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Mero_Doctor_Project.Models.Enums;
using Microsoft.EntityFrameworkCore;
using payment_gateway_nepal;
using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using static payment_gateway_nepal.ApiEndPoints;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net.WebSockets;
using System.Runtime.Intrinsics.X86;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ApplicationDbContext _context;

        public AppointmentsController(IAppointmentRepository appointmentRepository,ApplicationDbContext context)
        {
            _appointmentRepository = appointmentRepository;
            _context =context;
        }

        [HttpPost("bookAppointment")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentDto dto)
        {
            string patientUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (patientUserId == null)
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Unauthorized: Plesase Login to Book Appointment.",
                    Data = null
                });

            var result = await _appointmentRepository.BookAppointmentAsync(dto, patientUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpPut("update-status")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Doctor")]
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
        [HttpGet("doctorAppointments")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Doctor")]
        public async Task<IActionResult> GetAppointmentsByDoctor()
        {
            string doctorUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (doctorUserId == null)
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Unauthorized: Doctor ID not found",
                    Data = null
                });
            }
            var result = await _appointmentRepository.GetAppointmentsByDoctorAsync(doctorUserId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Patient: Get all appointments
        [HttpGet("patient")]
      [Authorize(AuthenticationSchemes = "Bearer", Roles = "Patient")]
        public async Task<IActionResult> GetAppointmentsByPatient()
        {
            var patientUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (patientUserId == null)
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Unauthorized: Patient ID not found",
                    Data = null
                });
            }

            var result = await _appointmentRepository.GetAppointmentsByPatientAsync(patientUserId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentConfirmationDto dto)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.TransactionId == dto.TransactionId);
            if (appointment == null) return 
            NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "Appointment not found",
                Data = null
            });

            appointment.Status = AppointmentStatus.Accepted;
            appointment.TransactionStatus = "Complete";
            appointment.PaymentDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok("Payment confirmed");
        }


        [HttpPost("fail-payment")]
        public async Task<IActionResult> FailPayment([FromBody] PaymentConfirmationDto dto)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.TransactionId == dto.TransactionId);

            if (appointment == null)
                return NotFound("Appointment not found");

            appointment.Status = AppointmentStatus.Rejected;
            appointment.TransactionStatus = "Failed";
            await _context.SaveChangesAsync();

            return Ok("Payment marked as failed");
        }


    }
}
