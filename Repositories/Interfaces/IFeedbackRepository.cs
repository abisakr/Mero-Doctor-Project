using Mero_Doctor_Project.DTOs.FeedbackDto;
using Mero_Doctor_Project.Models.Common;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<ResponseModel<string>> CreateFeedbackAsync(FeedbackCreateDto dto);
        Task<ResponseModel<List<FeedbackGetDto>>> GetAllFeedbacksAsync();
    }
}
