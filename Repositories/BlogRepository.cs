using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.BlogsDto;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;
        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<string>> AddAsync(BlogAddDto dto, string userId)
        {
            try
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
                if (doctor == null)
                    return new ResponseModel<string> { Success = false, Message = "Doctor not found" };
                var category = await _context.Categories.FindAsync(dto.CategoryId);
                    if (category == null)
                    return new ResponseModel<string> { Success = false, Message = "Category not found" };
                var blog = new Blog
                {
                    Title = dto.Title,
                    Content = dto.Content,
                    CreatedDate = DateTime.UtcNow,
                    DoctorId = doctor.DoctorId,
                    CategoryId = dto.CategoryId
                };

                _context.Blogs.Add(blog);
                await _context.SaveChangesAsync();

                return new ResponseModel<string> { Success = true, Message = "Blog added successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<string>> UpdateAsync(BlogUpdateDto dto, string userId)
        {
            try
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
                if (doctor == null)
                    return new ResponseModel<string> { Success = false, Message = "Doctor not found" };

                var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.BlogId == dto.BlogId && b.DoctorId == doctor.DoctorId);
                if (blog == null)
                    return new ResponseModel<string> { Success = false, Message = "Blog not found or unauthorized" };

                blog.Title = dto.Title;
                blog.Content = dto.Content;
                blog.CategoryId = dto.CategoryId;
                await _context.SaveChangesAsync();

                return new ResponseModel<string> { Success = true, Message = "Blog updated successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<string>> DeleteAsync(int blogId, string userId)
        {
            try
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
                if (doctor == null)
                    return new ResponseModel<string> { Success = false, Message = "Doctor not found" };
                var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.BlogId == blogId && b.DoctorId == doctor.DoctorId);
                if (blog == null)
                    return new ResponseModel<string> { Success = false, Message = "Blog not found or unauthorized" };

                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();

                return new ResponseModel<string> { Success = true, Message = "Blog deleted successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<BlogGetDto>> GetAsync(int blogId)
        {
            try
            {
                var blog = await _context.Blogs
                    .Include(b => b.Doctor).ThenInclude(d => d.User)
                    .Include(b => b.Category)
                    .Include(b => b.Likes)
                    .FirstOrDefaultAsync(b => b.BlogId == blogId);

                if (blog == null)
                    return new ResponseModel<BlogGetDto> { Success = false, Message = "Blog not found" };

                var dto = new BlogGetDto
                {
                    BlogId = blog.BlogId,
                    Title = blog.Title,
                    Content = blog.Content,
                    CreatedDate = blog.CreatedDate,
                    CategoryName = blog.Category?.Name,
                    DoctorName = blog.Doctor?.User?.FullName,
                    TotalLikes = blog.Likes?.Count ?? 0
                };

                return new ResponseModel<BlogGetDto> { Success = true, Message = "Blog fetched", Data = dto };
            }
            catch (Exception ex)
            {
                return new ResponseModel<BlogGetDto> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<BlogGetAllDto>> GetAllAsync()
        {
            try
            {
                var blogs = await _context.Blogs
                    .Include(b => b.Doctor).ThenInclude(d => d.User)
                    .Include(b => b.Category)
                    .Include(b => b.Likes)
                    .OrderByDescending(b => b.CreatedDate)
                    .ToListAsync();

                var dto = new BlogGetAllDto
                {
                    Blogs = blogs.Select(blog => new BlogGetDto
                    {
                        BlogId = blog.BlogId,
                        Title = blog.Title,
                        Content = blog.Content,
                        CreatedDate = blog.CreatedDate,
                        CategoryName = blog.Category?.Name,
                        DoctorName = blog.Doctor?.User?.FullName,
                        TotalLikes = blog.Likes?.Count ?? 0
                    }).ToList()
                };

                return new ResponseModel<BlogGetAllDto> { Success = true, Data = dto };
            }
            catch (Exception ex)
            {
                return new ResponseModel<BlogGetAllDto> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<BlogGetAllDto>> GetBlogsByDoctorAsync(string userId)
        {
            try
            {
                var doctor = await _context.Doctors.Include(d => d.User).FirstOrDefaultAsync(d => d.UserId == userId);
                if (doctor == null)
                    return new ResponseModel<BlogGetAllDto> { Success = false, Message = "Doctor not found" };

                var blogs = await _context.Blogs
                    .Where(b => b.DoctorId == doctor.DoctorId)
                    .Include(b => b.Category)
                    .Include(b => b.Likes)
                    .OrderByDescending(b => b.CreatedDate)
                    .ToListAsync();

                var dto = new BlogGetAllDto
                {
                    Blogs = blogs.Select(blog => new BlogGetDto
                    {
                        BlogId = blog.BlogId,
                        Title = blog.Title,
                        Content = blog.Content,
                        CreatedDate = blog.CreatedDate,
                        CategoryName = blog.Category?.Name,
                        DoctorName = doctor.User?.FullName,
                        TotalLikes = blog.Likes?.Count ?? 0
                    }).ToList()
                };

                return new ResponseModel<BlogGetAllDto> { Success = true, Data = dto };
            }
            catch (Exception ex)
            {
                return new ResponseModel<BlogGetAllDto> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

    }
 
}
