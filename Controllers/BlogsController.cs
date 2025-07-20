using System.Security.Claims;
using Mero_Doctor_Project.DTOs.BlogsDto;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;

        public BlogsController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        [HttpPost("Add")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AddBlog( [FromForm] BlogAddDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _blogRepository.AddAsync(dto, userId);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("Update")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateBlog([FromForm] BlogUpdateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _blogRepository.UpdateAsync(dto, userId);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _blogRepository.DeleteAsync(id, userId);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetBlog(int id)
        {
            var result = await _blogRepository.GetAsync(id);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllBlogs()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var result = await _blogRepository.GetAllAsync(userId);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }


        [HttpGet("GetByDoctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorBlogs()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _blogRepository.GetBlogsByDoctorAsync(userId);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
    }
}
