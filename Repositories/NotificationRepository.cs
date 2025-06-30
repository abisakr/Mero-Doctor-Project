using Mero_Doctor_Project.Data;
using Mero_Doctor_Project.DTOs.FeedbackDto;
using Mero_Doctor_Project.DTOs.NotificationDto;
using Mero_Doctor_Project.DTOs.Specilization;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Models.Common;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Repositories
{
    public class NotificationRepository: Repository<Notification>,INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ResponseModel<string>> AddAsync(NotificationCreateDto dto,string userId)
        {
            try
            {
                var notification = new Notification
                {
                    Message = dto.Message,
                    UserId = userId,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.Notifications.AddAsync(notification);
                var result=await _context.SaveChangesAsync();

                if (result<=0)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to save notification.",
                        Data = null
                    };
                }

                return new ResponseModel<string>
                {
                    Success = true,
                    Message = "Notification Saved.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
        public async Task<ResponseModel<List<NotificationViewDto>>> GeAllNotificationsByIdAsync(string userId)
        {
            try
            {
                var notification =  _context.Notifications.Where(a=>a.UserId==userId).OrderByDescending(a=>a.CreatedAt);
                if (notification == null)
                {
                    return new ResponseModel<List<NotificationViewDto>>
                    {
                        Success = false,
                        Message = "No Notifications.",
                        Data = null
                    };
                }

                var result = notification.Select(a => new NotificationViewDto
                {
                    NotificationId = a.NotificationId,
                    Message = a.Message,
                    IsRead = a.IsRead,
                    CreatedAt = a.CreatedAt
                }).ToList();

                return new ResponseModel<List<NotificationViewDto>>
                {
                    Success = true,
                    Message = "Notifications Found.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<NotificationViewDto>  >
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
        // Implement methods for notification repository here
    }
}
