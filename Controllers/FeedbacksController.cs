using Mero_Doctor_Project.DTOs.FeedbackDto;
using Mero_Doctor_Project.Repositories;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbacksController(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackCreateDto dto)
        {
            var result = await _feedbackRepository.CreateFeedbackAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var result = await _feedbackRepository.GetAllFeedbacksAsync();
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
