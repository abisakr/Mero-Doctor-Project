using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Mero_Doctor_Project.Repositories.DoctorWeeklyAvailabilityRepository;

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
    }
}
