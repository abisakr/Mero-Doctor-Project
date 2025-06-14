using System.Security.Claims;
using Mero_Doctor_Project.DTOs.DoctorDto;
using Mero_Doctor_Project.Repositories;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mero_Doctor_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorWeeklyAvailabilityRepository _doctorAvailabilityRepository;
        private readonly IDoctorRepository _doctor;

        public DoctorController(IDoctorWeeklyAvailabilityRepository doctorAvailabilityRepository,IDoctorRepository doctor)
        {
            _doctorAvailabilityRepository = doctorAvailabilityRepository;
            _doctor = doctor;
        }

        [HttpPost("SetAvailability")]
        public async Task<IActionResult> SetAvailability([FromBody] DoctorAvailabilityDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _doctorAvailabilityRepository.SetDoctorAvailabilityAsync(dto);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode(500, result);
        }

        [HttpGet("GetAvailability/{doctorId}")]
        public async Task<IActionResult> GetAvailability(int doctorId)
        {
            var result = await _doctorAvailabilityRepository.GetDoctorAvailabilityAsync(doctorId);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Doctor")]
        [HttpDelete("delete-weekday")]
        public async Task<IActionResult> DeleteWeekdayAvailability([FromBody] DeleteWeekdayDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var userId = "e6e43d3a-f925-44df-ba94-a84e1dd61157";
            var result = await _doctorAvailabilityRepository.DeleteDoctorWeekdayAsync(dto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Doctor")]
        [HttpDelete("delete-time-range")]
        public async Task<IActionResult> DeleteTimeRange([FromBody] DeleteTimeRangeDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var userId = "e6e43d3a-f925-44df-ba94-a84e1dd61157";
            var result = await _doctorAvailabilityRepository.DeleteDoctorTimeRangeAsync(dto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getDoctorById/{userId}")]
        public async Task<IActionResult> GetDoctorById(string userId)
        {
            var response = await _doctor.GetDoctorByIdAsync(userId);
            if (!response.Success)
                return NotFound(response);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Doctor")]
        [HttpGet("fetchDoctorOwnDetails")]
        public async Task<IActionResult> FetchDoctorOwnDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Please Login as Doctor.");
            var response = await _doctor.GetDoctorByIdAsync(userId);
            if (!response.Success)
                return NotFound(response);
            return Ok(response);
        }
   
        [HttpGet("filter")]
        public async Task<IActionResult> GetDoctorsByFilter([FromQuery] int? specializationId, [FromQuery] string? doctorName)
        {
            var response = await _doctor.GetDoctorsByFilterAsync(specializationId, doctorName);
            if (!response.Success || response.Data == null || response.Data.Count == 0)
                return NotFound(response);
            return Ok(response);
        }
        [HttpGet("getAllDoctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctor.GetAllDoctorsAsync();

            if (doctors.Data.Count==0)
            {
                return NotFound(doctors);
            }

            return Ok(doctors);
        }
    }
}
