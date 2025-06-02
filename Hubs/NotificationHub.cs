using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Mero_Doctor_Project.Hubs
{
    public class NotificationHub : Hub
    {
        // Make this public so your repository can use it
        public static readonly ConcurrentDictionary<string, List<string>> UserConnections = new();

        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections.AddOrUpdate(userId,
                    new List<string> { Context.ConnectionId },
                    (key, existingList) =>
                    {
                        if (!existingList.Contains(Context.ConnectionId))
                            existingList.Add(Context.ConnectionId);
                        return existingList;
                    });
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId) && UserConnections.ContainsKey(userId))
            {
                UserConnections[userId].Remove(Context.ConnectionId);
                if (UserConnections[userId].Count == 0)
                    UserConnections.TryRemove(userId, out _);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
