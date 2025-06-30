using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mero_Doctor_Project.DTOs.BlogsDto;

namespace Mero_Doctor_Project.Repositories
{
    public class BlogCommentRepository : IBlogCommentRepository
    {
        private readonly ApplicationDbContext _context;
        public BlogCommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<string>> AddCommentAsync(BlogCommentAddDto dto, string userId, string userName)
        {
            try
            {
                var comment = new BlogComment
                {
                    BlogId = dto.BlogId,
                    Comment = dto.Comment,
                    UserId = userId,
                    Name = userName,
                    CreatedDate = DateTime.UtcNow
                };
                var doctor=await _context.Blogs.Include(a=>a.Doctor).FirstOrDefaultAsync(b => b.BlogId == dto.BlogId);   
                _context.BlogComments.Add(comment);
                await _context.SaveChangesAsync();

                return new ResponseModel<string> { Success = true, Message = "Comment added.",Data=doctor.Doctor.UserId };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<string>> UpdateCommentAsync(BlogCommentUpdateDto dto, string userId)
        {
            try
            {
                var comment = await _context.BlogComments.FirstOrDefaultAsync(c => c.BlogCommentId == dto.BlogCommentId && c.UserId == userId);
                if (comment == null)
                    return new ResponseModel<string> { Success = false, Message = "Comment not found or access denied." };

                comment.Comment = dto.Comment;
                await _context.SaveChangesAsync();

                return new ResponseModel<string> { Success = true, Message = "Comment updated." };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<string>> DeleteCommentAsync(int commentId, string userId)
        {
            try
            {
                var comment = await _context.BlogComments.FirstOrDefaultAsync(c => c.BlogCommentId == commentId && c.UserId == userId);
                if (comment == null)
                    return new ResponseModel<string> { Success = false, Message = "Comment not found or access denied." };

                _context.BlogComments.Remove(comment);
                await _context.SaveChangesAsync();

                return new ResponseModel<string> { Success = true, Message = "Comment deleted." };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<List<BlogCommentGetDto>>> GetCommentsByBlogIdAsync(int blogId)
        {
            var comments = await _context.BlogComments
                .Where(c => c.BlogId == blogId)
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => new BlogCommentGetDto
                {
                    BlogCommentId = c.BlogCommentId,
                    BlogId = c.BlogId,
                    Comment = c.Comment,
                    Name = c.Name,
                    CreatedDate = c.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss tt")  
                }).ToListAsync();

            return new ResponseModel<List<BlogCommentGetDto>>
            {
                Success = true,
                Message = "Comments fetched.",
                Data = comments
            };
        }

        public async Task<ResponseModel<List<BlogCommentGetDto>>> GetCommentsByUserAsync(string userId)
        {
            var comments = await _context.BlogComments
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => new BlogCommentGetDto
                {
                    BlogCommentId = c.BlogCommentId,
                    BlogId = c.BlogId,
                    Comment = c.Comment,
                    Name = c.Name,
                    CreatedDate = c.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss tt")  
                }).ToListAsync();

            return new ResponseModel<List<BlogCommentGetDto>>
            {
                Success = true,
                Message = "User comments fetched.",
                Data = comments
            };
        }
    }
    
}
