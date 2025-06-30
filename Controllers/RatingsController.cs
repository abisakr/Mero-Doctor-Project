using Mero_Doctor_Project.DTOs.DoctorDto;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mero_Doctor_Project.Repositories;
using Mero_Doctor_Project.Helper;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingReviewRepository _ratingReviewRepository;
        private readonly NotificationHelper _notificationHelper;

        public RatingsController(IRatingReviewRepository ratingReviewRepository, NotificationHelper notificationHelper)
        {
            _ratingReviewRepository = ratingReviewRepository;
            _notificationHelper = notificationHelper;

        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] RatingReviewCreateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _ratingReviewRepository.CreateRatingAsync(dto, userId);
            if (result.Success)
            {
              await  _notificationHelper.SendAndStoreNotificationAsync(result.Data, "You received a new rating from a patient.");
                return Ok(new ResponseModel<string> { Success = true, Message = "Rating added successfully." });
            }
            return BadRequest(result);
        }

        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] RatingReviewUpdateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _ratingReviewRepository.UpdateRatingAsync(dto, userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return  BadRequest(result);
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
