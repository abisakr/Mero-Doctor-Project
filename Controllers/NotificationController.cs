using Mero_Doctor_Project.DTOs.DoctorDto;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mero_Doctor_Project.Repositories;
using Mero_Doctor_Project.DTOs.NotificationDto;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create([FromBody] NotificationCreateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _notificationRepository.AddAsync(dto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("notifications")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GeAllNotificationsByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _notificationRepository.GeAllNotificationsByIdAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
