using Mero_Doctor_Project.DTOs.PneumoniaDetectionDto;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using System.Threading.Tasks;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IXRayRecordRepository
    {
        Task<ResponseModel<XRayLiveHistoryDto>> DetectPneumonia(IFormFile xrayImage, string userId);

        Task<ResponseModel<List<GetXRayHistoryDto>>> GetUserXRayHistory(int? patientId, string currentUserId);
    }
}