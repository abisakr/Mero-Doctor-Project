using Mero_Doctor_Project.DTOs.DoctorDto;
using Mero_Doctor_Project.Models.Common;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IRatingReviewRepository
    {
        Task<ResponseModel<string>> CreateRatingAsync(RatingReviewCreateDto dto, string userId);
        Task<ResponseModel<string>> UpdateRatingAsync(RatingReviewUpdateDto dto, string userId);
        Task<ResponseModel<RatingReviewGetDto>> GetUserRatingForDoctorAsync(int doctorId, string userId);
        Task<ResponseModel<DoctorRatingSummaryDto>> GetAllRatingsForDoctorAsync(int doctorId);
    }
}
