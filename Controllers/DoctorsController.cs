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

        public DoctorController(IDoctorWeeklyAvailabilityRepository doctorAvailabilityRepository)
        {
            _doctorAvailabilityRepository = doctorAvailabilityRepository;
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

        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Doctor")]
        [HttpDelete("delete-time-range")]
        public async Task<IActionResult> DeleteTimeRange([FromBody] DeleteTimeRangeDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var userId = "e6e43d3a-f925-44df-ba94-a84e1dd61157";
            var result = await _doctorAvailabilityRepository.DeleteDoctorTimeRangeAsync(dto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
