using Mero_Doctor_Project.DTOs.NotificationDto;
using Mero_Doctor_Project.Hubs;
using Mero_Doctor_Project.Models;
using Mero_Doctor_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace Mero_Doctor_Project.Helper
{
    public class NotificationHelper
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationRepository _notificationRepository;

        public NotificationHelper(IHubContext<NotificationHub> hubContext, UserManager<ApplicationUser> userManager,INotificationRepository notificationRepository)
        {
            _hubContext = hubContext;
            _userManager = userManager;
            _notificationRepository = notificationRepository;
        }


        public async Task NotifyUserAsync(string userId, string message)
        {
            if (NotificationHub.UserConnections.TryGetValue(userId, out var connections) && connections.Any())
            {
                var tasks = connections.Select(async connectionId =>
                {
                    try
                    {
                        await _hubContext.Clients.Client(connectionId)
                            .SendAsync("ReceiveNotification", "System", message);
                        Console.WriteLine($" Notification sent to connection: {connectionId} | Message: {message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Failed to notify user (ID: {userId}) at connection {connectionId}: {ex.Message}");
                    }
                });

                await Task.WhenAll(tasks);
            }
            else
            {
                Console.WriteLine($" User {userId} is offline. Notification stored only.");
            }
        }


        public async Task SendAndStoreNotificationAsync(string userId, string message)
        {
            await _notificationRepository.AddAsync(new NotificationCreateDto { Message = message }, userId);
            await NotifyUserAsync(userId, message);
         }

    }

}
