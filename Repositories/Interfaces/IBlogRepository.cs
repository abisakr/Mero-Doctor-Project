using Mero_Doctor_Project.DTOs.BlogsDto;
using Mero_Doctor_Project.Models.Common;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IBlogRepository
    {
        Task<ResponseModel<string>> AddAsync(BlogAddDto blogAddDto, string userId);
        Task<ResponseModel<string>> UpdateAsync(BlogUpdateDto blogUpdateDto, string userId);
        Task<ResponseModel<string>> DeleteAsync(int blogId, string userId);
        Task<ResponseModel<BlogGetDto>> GetAsync(int blogId);
        Task<ResponseModel<BlogGetAllDto>> GetAllAsync();
        Task<ResponseModel<BlogGetAllDto>> GetBlogsByDoctorAsync(string userId);
    }
}
