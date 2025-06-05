using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mero_Doctor_Project.DTOs.BlogsDto;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCommentsController : ControllerBase
    {
        private readonly IBlogCommentRepository _repo;

        public BlogCommentsController(IBlogCommentRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] BlogCommentAddDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
         
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = await _repo.AddCommentAsync(dto, userId, userName);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateComment([FromBody] BlogCommentUpdateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _repo.UpdateCommentAsync(dto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _repo.DeleteCommentAsync(id, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("ByBlog/{blogId}")]
        public async Task<IActionResult> GetCommentsByBlog(int blogId)
        {
            var result = await _repo.GetCommentsByBlogIdAsync(blogId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("MyComments")]
        [Authorize]
        public async Task<IActionResult> GetCommentsByUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _repo.GetCommentsByUserAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
