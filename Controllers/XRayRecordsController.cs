using Mero_Doctor_Project.DTOs.PneumoniaDetectionDto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mero_Doctor_Project.Repositories.Interfaces;
using static System.Net.WebRequestMethods;
using Mero_Doctor_Project.Repositories;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XRayRecordsController : ControllerBase
    {
        private readonly IXRayRecordRepository _xRayRecordRepository;
        private readonly PatientRepository _patientRepository;

        public XRayRecordsController(IXRayRecordRepository xRayRecordRepository, PatientRepository patientRepository)
        {
            _xRayRecordRepository = xRayRecordRepository;
            _patientRepository = patientRepository;
        }

        [Authorize]
        [HttpPost("detect-pneumonia")]
        public async Task<IActionResult> DetectPneumonia([FromForm] DetectPneumoniaDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var result = await _xRayRecordRepository.DetectPneumonia(dto.XRayImage, userId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("xray-history")]
        public async Task<IActionResult> GetXRayHistory([FromQuery] int? patientId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized("User session invalid.");

            var result = await _xRayRecordRepository.GetUserXRayHistory(patientId, currentUserId);
            return Ok(result);
        }
    }
}