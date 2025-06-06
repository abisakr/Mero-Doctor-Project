using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.PneumoniaDetectionDto;
using Mero_Doctor_Project.Helper;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Mero_Doctor_Project.Repositories
{
    public class XRayRecordRepository : IXRayRecordRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UploadImageHelper _uploadImageHelper;
        private readonly IWebHostEnvironment _environment;

        public XRayRecordRepository(ApplicationDbContext context,UploadImageHelper uploadImageHelper ,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _uploadImageHelper = uploadImageHelper;
        }
        public async Task<ResponseModel<string>> DetectPneumonia(IFormFile xrayImage, string userId)
        {
            try
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                if (patient == null)
                    return new ResponseModel<string> { Success = false, Message = "Patient not found." };

                string savedImageUrl = await _uploadImageHelper.UploadImageAsync(xrayImage, "xray-images");

                var localPath = Path.Combine(_environment.WebRootPath, savedImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

                using var client = new HttpClient();
                using var content = new MultipartFormDataContent();
                var stream = new FileStream(localPath, FileMode.Open);
                content.Add(new StreamContent(stream), "xray", Path.GetFileName(localPath));

                var response = await client.PostAsync("http://localhost:5000/predict", content);
                if (!response.IsSuccessStatusCode)
                    return new ResponseModel<string> { Success = false, Message = "AI server error." };

                var json = await response.Content.ReadAsStringAsync();
                dynamic resultObj = JsonConvert.DeserializeObject(json);

                var record = new XRayRecord
                {
                    PatientId = patient.PatientId,
                    XRayImageUrl = savedImageUrl,
                    Result = resultObj.result,
                    GradCamUrl = resultObj.gradCamUrl,
                    RecommendedHospital = resultObj.result == "Pneumonia Detected" ? "Shree Harsha Hospital" : "No recommendation",
                    DateTime = DateTime.Now
                };

                _context.XRayRecords.Add(record);
                await _context.SaveChangesAsync();

                return new ResponseModel<string>
                {
                    Success = true,
                    Message = "X-ray processed successfully.",
                    Data = resultObj.result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }
        public async Task<ResponseModel<List<XRayHistoryDto>>> GetUserXRayHistory(string userId)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null)
            {
                return new ResponseModel<List<XRayHistoryDto>>
                {
                    Success = false,
                    Message = "Patient not found"
                };
            }

            var records = await _context.XRayRecords
                .Where(r => r.PatientId == patient.PatientId)
                .OrderByDescending(r => r.DateTime)
                .Select(r => new XRayHistoryDto
                {
                    XRayImageUrl = r.XRayImageUrl,
                    Result = r.Result,
                    GradCamUrl = r.GradCamUrl,
                    RecommendedHospital = r.RecommendedHospital,
                    DateTime = r.DateTime
                })
                .ToListAsync();

            return new ResponseModel<List<XRayHistoryDto>>
            {
                Success = true,
                Data = records
            };
        }

    }

}
