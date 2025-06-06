using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Repositories.Interfaces;
using Mero_Doctor_Project.DTOs.BlogsDto;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<string>> AddAsync(BlogCategoryAddDto dto)
        {
            try
            {
                var category = new Category { Name = dto.Name };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return new ResponseModel<string> { Success = true, Message = "Category added successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<string>> UpdateAsync(BlogCategoryUpdateDto dto)
        {
            try
            {
                var category = await _context.Categories.FindAsync(dto.CategoryId);
                if (category == null)
                    return new ResponseModel<string> { Success = false, Message = "Category not found." };

                category.Name = dto.Name;
                await _context.SaveChangesAsync();

                return new ResponseModel<string> { Success = true, Message = "Category updated successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<string>> DeleteAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return new ResponseModel<string> { Success = false, Message = "Category not found." };

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return new ResponseModel<string> { Success = true, Message = "Category deleted successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<List<BlogCategoryGetDto>>> GetAllAsync()
        {
            try
            {
                var categories = await _context.Categories
                    .Select(c => new BlogCategoryGetDto
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name
                    })
                    .ToListAsync();

                return new ResponseModel<List<BlogCategoryGetDto>>
                {
                    Success = true,
                    Message = "Categories fetched successfully.",
                    Data = categories
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<BlogCategoryGetDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
    }
    
}
