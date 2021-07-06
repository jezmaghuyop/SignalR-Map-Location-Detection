using Microsoft.AspNetCore.SignalR;
using SignalR.MapMarker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.MapMarker.Hubs
{
    public class MapHub : Hub
    {
        public static List<UserInfo> allUser = new List<UserInfo>();

        public async Task SendInfo(UserInfo userData)
        {
            // Include ConnectionId
            userData.ConnectionId = Context.ConnectionId;

            // Add User Info to the master list
            allUser.Add(userData);

            await Clients.Others.SendAsync("sendNotification", userData);
        }

        public async Task ViewUser(string recipient, string sender)
        {
            var recipientUser = allUser.FirstOrDefault(x => x.Email.Equals(recipient, StringComparison.OrdinalIgnoreCase));

            if (recipientUser != null)
            {
                await Clients.Clients(recipientUser.ConnectionId).SendAsync("viewNotification", sender);
            }
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("getAllUser", allUser);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (allUser.Count > 0)
            {
                // Find Disconnected User
                var user = allUser.Find(x => x.ConnectionId == Context.ConnectionId);

                if (user != null)
                {
                    // Remove disconnected User from the list
                    allUser.Remove(user);

                    // Inform Everyone that a user has been disconnected, and remove it from the Map
                    await Clients.Others.SendAsync("userDisconnected", user);
                }

            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
