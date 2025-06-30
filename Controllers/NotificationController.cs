using Mero_Doctor_Project.DTOs.DoctorDto;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IRatingReviewRepository _ratingReviewRepository;

        public NotificationController(IRatingReviewRepository ratingReviewRepository)
        {
            _ratingReviewRepository = ratingReviewRepository;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] RatingReviewCreateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _ratingReviewRepository.CreateRatingAsync(dto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("DoctorRatings/{doctorId}")]
        public async Task<IActionResult> GetDoctorRatings(int doctorId)
        {
            var result = await _ratingReviewRepository.GetAllRatingsForDoctorAsync(doctorId);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
