using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.BlogsDto;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mero_Doctor_Project.Helper;

namespace Mero_Doctor_Project.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly NotificationHelper _notificationHelper;

        public LikeRepository(ApplicationDbContext context, NotificationHelper notificationHelper)
        {
            _context = context;
            _notificationHelper = notificationHelper;

        }

        public async Task<ResponseModel<string>> ToggleLikeAsync(LikeToggleDto dto, string userId, string userName)
        {
            try
            {
                var existingLike = await _context.Likes.FirstOrDefaultAsync(l =>
                    l.BlogId == dto.BlogId && l.UserId == userId);
                var doctor = await _context.Blogs.Include(a => a.Doctor).FirstOrDefaultAsync(b => b.BlogId == dto.BlogId);
                if (existingLike != null)
                {
                    // User already liked → unlike by deleting the record
                    _context.Likes.Remove(existingLike);
                    await _context.SaveChangesAsync();

                    return new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Unliked successfully."
                    };
                }
                else
                {
                    // Not liked yet → add new like
                    var like = new Like
                    {
                        BlogId = dto.BlogId,
                        UserId = userId,
                        Name = userName,
                        LikedDate = DateTime.UtcNow
                    };

                    _context.Likes.Add(like);
                    await _context.SaveChangesAsync();
                    string message = "Your blog received a new like.";
                    await _notificationHelper.SendAndStoreNotificationAsync(doctor.Doctor.UserId, message);
                    return new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Liked successfully."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
   
}
