using Mero_Doctor_Project.DTOs.BlogsDto;
using Mero_Doctor_Project.Models.Common;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface IBlogCommentRepository
    {
        Task<ResponseModel<string>> AddCommentAsync(BlogCommentAddDto dto, string userId, string userName);
        Task<ResponseModel<string>> UpdateCommentAsync(BlogCommentUpdateDto dto, string userId);
        Task<ResponseModel<string>> DeleteCommentAsync(int commentId, string userId);
        Task<ResponseModel<List<BlogCommentGetDto>>> GetCommentsByBlogIdAsync(int blogId);
        Task<ResponseModel<List<BlogCommentGetDto>>> GetCommentsByUserAsync(string userId);
    }
}
