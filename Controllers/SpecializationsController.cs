using Mero_Doctor_Project.DTOs.Specilization;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {

        //get all specialization with name and id  and display in client dropdown in doctor registration

        private readonly ISpecializationRepository _specializationRepository;

        public SpecializationsController(ISpecializationRepository specializationRepository)
        {
            _specializationRepository = specializationRepository;
        }

        // GET: api/Specialization
        [HttpGet]
        public async Task<ActionResult<ResponseModel<List<Specialization>>>> GetAll()
        {
            var result = await _specializationRepository.GetAllAsync();
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        // GET: api/Specialization/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<Specialization>>> GetById(int id)
        {
            var result = await _specializationRepository.GetByIdAsync(id);
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        // POST: api/Specialization
        [HttpPost]
        public async Task<ActionResult<ResponseModel<Specialization>>> Add([FromBody] SpecializationDto specializationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Validate incoming model
            }

            var result = await _specializationRepository.AddAsync(specializationDto);
            if (result.Success)
                return CreatedAtAction(nameof(GetById), new { id = result.Data.SpecializationId }, result);

            return BadRequest(result);
        }

        // PUT: api/Specialization/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseModel<Specialization>>> Update(int id, [FromBody] SpecializationDto specializationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Validate incoming model
            }

            var result = await _specializationRepository.UpdateAsync(id, specializationDto);
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        // DELETE: api/Specialization/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel<bool>>> Delete(int id)
        {
            var result = await _specializationRepository.DeleteAsync(id);
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
    }
}
