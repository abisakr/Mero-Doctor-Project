using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.DTOs.PneumoniaDetectionDto;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IXRayRecordRepository
    {
        Task<ResponseModel<string>> DetectPneumonia(IFormFile xrayImage, string userId);
        Task<ResponseModel<List<XRayHistoryDto>>> GetUserXRayHistory(string userId);


    }
}
