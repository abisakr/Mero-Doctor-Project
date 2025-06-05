using Mero_Doctor_Project.DTOs.BlogsDto;
using Mero_Doctor_Project.Repositories;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public BlogCategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost("Add")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] BlogCategoryAddDto dto)
        {
            var result = await _categoryRepository.AddAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("Update")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] BlogCategoryUpdateDto dto)
        {
            var result = await _categoryRepository.UpdateAsync(dto);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        [HttpDelete("Delete/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryRepository.DeleteAsync(id);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryRepository.GetAllAsync();
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
