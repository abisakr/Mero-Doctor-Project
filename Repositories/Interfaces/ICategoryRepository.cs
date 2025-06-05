using Mero_Doctor_Project.DTOs.BlogsDto;
using Mero_Doctor_Project.Models.Common;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<ResponseModel<string>> AddAsync(BlogCategoryAddDto dto);
        Task<ResponseModel<string>> UpdateAsync(BlogCategoryUpdateDto dto);
        Task<ResponseModel<string>> DeleteAsync(int id);
        Task<ResponseModel<List<BlogCategoryGetDto>>> GetAllAsync();
    }
}
