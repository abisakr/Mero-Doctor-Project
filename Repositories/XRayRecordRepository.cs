using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.PneumoniaDetectionDto;
using Mero_Doctor_Project.Helper;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public async Task<ResponseModel<XRayLiveHistoryDto>> DetectPneumonia(IFormFile xrayImage, string userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userId);
                if (user == null)
                    return new ResponseModel<XRayLiveHistoryDto> { Success = false, Message = "User not found." };

                var savedImageUrl = await _uploadImageHelper.UploadImageAsync(xrayImage, "xray-images");
                var localPath = Path.Combine(_environment.WebRootPath, savedImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

                using var client = new HttpClient();
                using var content = new MultipartFormDataContent();
                var stream = new FileStream(localPath, FileMode.Open);
                content.Add(new StreamContent(stream), "xray", Path.GetFileName(localPath));
                content.Add(new StringContent(user.Latitude.ToString()), "latitude");
                content.Add(new StringContent(user.Longitude.ToString()), "longitude");

                var response = await client.PostAsync("http://localhost:5000/predict", content);
                if (!response.IsSuccessStatusCode)
                    return new ResponseModel<XRayLiveHistoryDto> { Success = false, Message = "AI server error." };

                var json = await response.Content.ReadAsStringAsync();
                dynamic resultObj = JsonConvert.DeserializeObject(json);

                string result = resultObj.result;
                string gradcamUrl = resultObj.gradCamUrl;

                var recommendedHospitals = new List<HospitalRecommendationDto>();
              

                if (result == "Pneumonia Detected" && resultObj.recommendations is JArray recArray)
                {
                    foreach (var r in recArray)
                    {
                        if (r.Type == JTokenType.Object)
                        {
                            recommendedHospitals.Add(new HospitalRecommendationDto
                            {
                                Hospital = r["hospital"]?.ToString(),
                                Province = r["province"]?.ToString(),
                                Latitude = (double?)r["latitude"] ?? 0,
                                Longitude = (double?)r["longitude"] ?? 0,
                                Distance_km = (double?)r["distance_km"] ?? 0
                            });
                        }
                    }
                }

                var topHospital = (result == "Pneumonia Detected" && recommendedHospitals.Any())
     ? $"[{recommendedHospitals[0].Hospital}, {recommendedHospitals[0].Province}, {recommendedHospitals[0].Distance_km} km]"
     : "No recommendation";

                var record = new XRayRecord
                {
                    PatientId = user.Id,
                    XRayImageUrl = savedImageUrl,
                    Result = result,
                    GradCamUrl = gradcamUrl,
                //    RecommendedHospital = topHospital,
                    RecommendedHospital = recommendedHospitals.Any()
            ? $"[{recommendedHospitals[0].Hospital}, {recommendedHospitals[0].Province}, {recommendedHospitals[0].Distance_km} km]"
            : "No recommendation",
                    DateTime = DateTime.Now
                };

                _context.XRayRecords.Add(record);
                await _context.SaveChangesAsync();

                return new ResponseModel<XRayLiveHistoryDto>
                {
                    Success = true,
                    Message = "X-ray processed successfully.",
                    Data = new XRayLiveHistoryDto
                    {
                        XRayImageUrl = record.XRayImageUrl,
                        Result = record.Result,
                        GradCamUrl = record.GradCamUrl,
                        RecommendedHospitals = recommendedHospitals
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<XRayLiveHistoryDto> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<List<GetXRayHistoryDto>>> GetUserXRayHistory(string userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userId);
                if (user == null)
                {
                    return new ResponseModel<List<GetXRayHistoryDto>>
                    {
                        Success = false,
                        Message = "Patient not found"
                    };
                }

                var records = await _context.XRayRecords
                    .Where(r => r.PatientId == user.Id)
                    .OrderByDescending(r => r.DateTime)
                    .Select(r => new GetXRayHistoryDto
                    {
                        XRayImageUrl = r.XRayImageUrl,
                        Result = r.Result,
                        GradCamUrl = r.GradCamUrl,
                        RecommendedHospital = r.RecommendedHospital,
                        DateTime = r.DateTime.ToString("yyyy-MM-dd hh:mm:ss tt")

                    })
                    .ToListAsync();

                return new ResponseModel<List<GetXRayHistoryDto>>
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = records
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<GetXRayHistoryDto>> { Success = false, Message = $"Error: {ex.Message}" };
            }
        
        }

    }

}
