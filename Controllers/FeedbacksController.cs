using Mero_Doctor_Project.DTOs.FeedbackDto;
using Mero_Doctor_Project.DTOs.NotificationDto;
using Mero_Doctor_Project.Helper;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Repositories;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly NotificationHelper _notificationHelper;
        public FeedbacksController(UserManager<ApplicationUser> userManager ,IFeedbackRepository feedbackRepository, NotificationHelper notificationHelper)
        {
            _userManager = userManager;
            _feedbackRepository = feedbackRepository; 
            _notificationHelper = notificationHelper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackCreateDto dto)
        {
            var result = await _feedbackRepository.CreateFeedbackAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            // Create notification message
            string message = $"New feedback received from: {dto.Email}";

            // Get all admins
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var tasks = admins.Select(admin =>
            _notificationHelper.SendAndStoreNotificationAsync(admin.Id, message));
            await Task.WhenAll(tasks);


            return Ok(result);
        }


        [HttpGet("getAllFeedbacks")]
        [Authorize]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var result = await _feedbackRepository.GetAllFeedbacksAsync();
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
