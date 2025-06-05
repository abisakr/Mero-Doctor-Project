using Mero_Doctor_Project.DTOs.BlogsDto;
using Mero_Doctor_Project.Models.Common;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface ILikeRepository
    {
        Task<ResponseModel<string>> ToggleLikeAsync(LikeToggleDto dto, string userId, string userName);
    }
}
