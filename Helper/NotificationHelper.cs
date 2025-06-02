using Mero_Doctor_Project.Hubs;
using Mero_Doctor_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace Mero_Doctor_Project.Helper
{
    public class NotificationHelper
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationHelper(IHubContext<NotificationHub> hubContext, UserManager<ApplicationUser> userManager)
        {
            _hubContext = hubContext;
            _userManager = userManager;
        }

        public async Task NotifyAdminsAsync(string message)
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");

            foreach (var admin in admins)
            {
                var userId = admin.Id;

                // Get all connection IDs from static NotificationHub dictionary
                if (NotificationHub.UserConnections.TryGetValue(userId, out var connections))
                {
                    foreach (var connectionId in connections)
                    {
                        try
                        {
                            await _hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveNotification", "System", message);
                        }
                        catch (Exception ex)
                        {
                            // Log the error (for debugging purposes)
                            Debug.WriteLine($"Failed to send notification to connection {connectionId}: {ex.Message}");
                            // Optionally: Log to a logging system like Serilog, NLog, etc.
                        }
                    }
                }
            }
        }
    }
}
