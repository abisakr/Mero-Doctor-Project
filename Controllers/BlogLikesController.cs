using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.BlogsDto;
using Microsoft.AspNetCore.Authorization;
using Mero_Doctor_Project.Repositories.Interfaces;
using Mero_Doctor_Project.Repositories;
using System.Security.Claims;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogLikesController : ControllerBase
    {
        private readonly ILikeRepository _likeRepository;

        public BlogLikesController(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        [HttpPost("ToggleLike")]
        [Authorize]
        public async Task<IActionResult> ToggleLike([FromBody] LikeToggleDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = await _likeRepository.ToggleLikeAsync(dto, userId, userName);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }




    }
}
