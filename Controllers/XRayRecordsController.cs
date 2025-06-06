using Mero_Doctor_Project.DTOs.PneumoniaDetectionDto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mero_Doctor_Project.Repositories.Interfaces;
using static System.Net.WebRequestMethods;

namespace Mero_Doctor_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XRayRecordsController : ControllerBase
    {
        private readonly IXRayRecordRepository _xRayRecordRepository;

        public XRayRecordsController(IXRayRecordRepository xRayRecordRepository)
        {
            _xRayRecordRepository = xRayRecordRepository;
        }
        [Authorize]
        [HttpPost("detect-pneumonia")]
        public async Task<IActionResult> DetectPneumonia([FromForm] DetectPneumoniaDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _xRayRecordRepository.DetectPneumonia(dto.XRayImage, userId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("xray-history")]
        public async Task<IActionResult> GetXRayHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _xRayRecordRepository.GetUserXRayHistory(userId);
            return Ok(result);
        }

        //       flutter code for that
        //        Future<void> uploadXRay(File file) async {
        //  var request = http.MultipartRequest(
        //    'POST',
        //    Uri.parse("http://localhost:5082/api/XRay/detect-pneumonia"),
        //  );

        //        request.headers['Authorization'] = 'Bearer your_jwt_token';
        //  request.files.add(await http.MultipartFile.fromPath('XRayImage', file.path));

        //  var res = await request.send();

        //  if (res.statusCode == 200) {
        //    final body = await res.stream.bytesToString();
        //        final json = jsonDecode(body);
        //        print("Result: ${json['data']}");
        //    } else {
        //    print("Failed: ${res.statusCode}");
        //}
        //}

    }
}
