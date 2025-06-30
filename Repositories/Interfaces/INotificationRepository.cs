using Mero_Doctor_Project.DTOs.NotificationDto;
using Mero_Doctor_Project.Models.Common;

namespace Mero_Doctor_Project.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<ResponseModel<string>> AddAsync(NotificationCreateDto dto, string userId);
        Task<ResponseModel<List<NotificationViewDto>>> GeAllNotificationsByIdAsync(string userId);
    }
}
