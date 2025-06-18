using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.FeedbackDto;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ApplicationDbContext _context;
        public FeedbackRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<string>> CreateFeedbackAsync(FeedbackCreateDto dto)
        {
            try
            {
                var feedback = new Feedback
                {
                    Email = dto.Email,
                    Description = dto.Description,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                return new ResponseModel<string> { Success = true, Message = "Feedback submitted successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<List<FeedbackGetDto>>> GetAllFeedbacksAsync()
        {
            var feedbacks = await _context.Feedbacks
                .OrderByDescending(f => f.CreatedDate)
                .Select(f => new FeedbackGetDto
                {
                    FeedbackId = f.FeedbackId,
                    Email = f.Email,
                    Description = f.Description,
                    CreatedDate = f.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss tt"),
                }).ToListAsync();

            return new ResponseModel<List<FeedbackGetDto>>
            {
                Success = true,
                Message = "Feedbacks retrieved.",
                Data = feedbacks
            };
        }
    }
   
}
