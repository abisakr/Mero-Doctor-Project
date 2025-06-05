using Mero_Doctor_Project.DTOs.DoctorDto;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mero_Doctor_Project.Repositories;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingReviewRepository _ratingReviewRepository;

        public RatingsController(IRatingReviewRepository ratingReviewRepository)
        {
            _ratingReviewRepository = ratingReviewRepository;
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] RatingReviewCreateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _ratingReviewRepository.CreateRatingAsync(dto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] RatingReviewUpdateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _ratingReviewRepository.UpdateRatingAsync(dto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("UserRating/{doctorId}")]
        [Authorize]
        public async Task<IActionResult> GetUserRating(int doctorId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _ratingReviewRepository.GetUserRatingForDoctorAsync(doctorId, userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("DoctorRatings/{doctorId}")]
        public async Task<IActionResult> GetDoctorRatings(int doctorId)
        {
            var result = await _ratingReviewRepository.GetAllRatingsForDoctorAsync(doctorId);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
