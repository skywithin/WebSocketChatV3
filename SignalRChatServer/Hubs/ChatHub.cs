using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChatServer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }

        public async Task AddUserToGroup(string user, string groupName)
        {
            // TODO: Check is the group is full

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var message = $"{user} has joined the group {groupName}.";

            await Clients.Group(groupName).SendAsync("GroupChanged", message);

            _logger.LogInformation($"{user} has joined the group {groupName}. ConnectionId: {Context.ConnectionId}");
        }

        public async Task RemoveUserFromGroup(string user, string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            var message = $"{user} has left the group {groupName}.";

            await Clients.Group(groupName).SendAsync("GroupChanged", message);

            _logger.LogInformation($"{user} has left the group {groupName}. ConnectionId: {Context.ConnectionId}");
        }

        public async Task SendMessageToGroup(string user, string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync(method: "ReceiveMessage", user, message);

            _logger.LogInformation($"Received message from {user} for group {groupName}. ConnectionId {Context.ConnectionId}");
        }
    }
}
