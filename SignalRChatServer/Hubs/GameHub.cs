using Application.MediatR.Commands.FindOrCreateUser;
using Application.MediatR.Commands.StoreChat;
using Application.MediatR.Queries.GetChatHistory;
using Application.Services;
using Application.Services.Models;
using Core.Messaging;
using Core.Messaging.Constants;
using Core.Messaging.Payloads;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChatServer.Hubs
{
    public class GameHub : Hub
    {
        private readonly ILogger<GameHub> _logger;
        private readonly IMediator _mediator;
        private readonly IChatGroupService _chatGroupService;

        public GameHub(
            ILogger<GameHub> logger,
            IMediator mediator,
            IChatGroupService chatGroupService)
        {
            _logger = logger;
            _mediator = mediator;
            _chatGroupService = chatGroupService;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"New connection {Context.ConnectionId}");
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            _logger.LogInformation($"Connection closed {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Echo(string message)
        {
            _logger.LogInformation($"Received echo request from {Context.ConnectionId}");

            await Clients.Client(Context.ConnectionId).SendAsync(ClientCommand.Echo, message);
        }

        public async Task HubLogin(Message<LoginRequestMessage> message)
        {
            //TODO: Proper implementation of authentication
            //TODO: Too much logic in here. Need to move it to service layer
            var msgPayload = message.Payload;

            _logger.LogInformation($"Received login request from {msgPayload.UserName}");

            var user = await _mediator.Send(new FindOrCreateUserCommand { UserName = msgPayload.UserName });

            // IF SUCCESS...
            _logger.LogInformation($"User login success. User ID {user.UserId}");
            
            var loginResultMsg =
                new MessageBuilder<LoginResultMessage>()
                    .WithPayload(
                        new LoginResultMessage
                        {
                            IsLoginSuccees = true, //For now always return true
                            UserId = user.UserId,
                            Groups =
                                user.Groups
                                    .Select(g =>
                                        new LoginResultMessage.ActiveGroup
                                        {
                                            GroupId = g.GroupId,
                                            GroupName = g.GroupName
                                        })
                                    .ToList(),
                        })
                    .Build();

            await Clients.Client(Context.ConnectionId).SendAsync(ClientCommand.LoginSuccess, loginResultMsg);

            if (user.Groups.Any())
            {
                //User is already in a group. Join group chat
                var joinGroupResult = 
                    await _chatGroupService.JoinGroup(
                        new JoinGroupRequest(
                            user.Groups.First().GroupName,
                            user.UserId));

                await ProcessJoinGroupResult(joinGroupResult, msgPayload.UserName);
            }
        }

        public async Task HubJoinGroup(Message<JoinGroupRequestMessage> message)
        {
            //TODO: Too much logic in here. Need to move it to service layer
            var msgPayload = message.Payload;

            _logger.LogInformation($"Received request from {msgPayload.UserName} to join group {msgPayload.GroupName}");

            var joinGroupResult = 
                await _chatGroupService.JoinGroup(new JoinGroupRequest(msgPayload.GroupName, msgPayload.UserId));

            await ProcessJoinGroupResult(joinGroupResult, msgPayload.UserName);
        }

        private async Task ProcessJoinGroupResult(JoinGroupResult joinGroupResult, string userName)
        {
            if (joinGroupResult.IsAllowed)
            {
                await NotifyOkToJoinGroup(joinGroupResult, userName);
            }
            else
            {
                // Notify about failure
                var deniedMsg =
                   new MessageBuilder<JoinGroupDeniedMessage>()
                       .WithPayload(new JoinGroupDeniedMessage(joinGroupResult.GroupName, joinGroupResult.DeniedReason))
                       .Build();

                await Clients.Client(Context.ConnectionId).SendAsync(ClientCommand.JoinGroupDenied, deniedMsg);
            }
        }

        private async Task NotifyOkToJoinGroup(JoinGroupResult joinGroupResult, string userName)
        {
            var groupName = joinGroupResult.GroupName;

            // Add user connection to the hub group
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            // Send notification to the user that join request has been accepted
            var groupAcceptedMsg =
                new MessageBuilder<JoinGroupAcceptedMessage>()
                    .WithPayload(new JoinGroupAcceptedMessage(joinGroupResult.GroupId, groupName))
                    .Build();

            await Clients.Client(Context.ConnectionId).SendAsync(ClientCommand.JoinGroupAccepted, groupAcceptedMsg);

            // Notify the group that user has joined
            var userJoinedMsg =
                new MessageBuilder<UserJoinedGroupMessage>()
                    .WithPayload(new UserJoinedGroupMessage(joinGroupResult.UserId, userName, groupName))
                    .Build();

            await Clients.Group(groupName).SendAsync(ClientCommand.UserJoinedGroup, userJoinedMsg);

            // Send chat message history to the new user
            var messageHistory = await _mediator.Send(new GetChatHistoryQuery(joinGroupResult.GroupId));

            foreach (var prevMsg in messageHistory.ChatHistory)
            {
                var previousChatMsg =
                    new MessageBuilder<ChatMessage>()
                        .WithPayload(
                            new ChatMessage(
                                prevMsg.UserId,
                                prevMsg.AuthorName,
                                prevMsg.GroupId,
                                groupName,
                                prevMsg.Content,
                                prevMsg.DateCreatedUtc))
                        .Build();

                await Clients.Client(Context.ConnectionId).SendAsync(ClientCommand.MessageToGroup, previousChatMsg);
            }

            await Clients.Client(Context.ConnectionId).SendAsync(ClientCommand.JoinGroupAccepted, groupAcceptedMsg);
        }

        public async Task HubLeaveGroup(Message<LeaveGroupMessage> message)
        {
            var msgPayload = message.Payload;

            _logger.LogInformation($"User {msgPayload.UserName} is leaving group {msgPayload.GroupName}");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, msgPayload.GroupName);

            var leaveMsg =
                new MessageBuilder<UserLeftGroupMessage>()
                    .WithPayload(new UserLeftGroupMessage { GroupName = msgPayload.GroupName, UserName = msgPayload.UserName })
                    .Build();

            await Clients.Group(msgPayload.GroupName).SendAsync(ClientCommand.UserLeftGroup, leaveMsg);

            await Clients.Client(Context.ConnectionId).SendAsync(ClientCommand.RemovedFromGroup, leaveMsg);
        }

        public async Task HubGroupChat(Message<ChatMessage> message)
        {
            var msgPayload = message.Payload;

            _logger.LogInformation($"Received message from {msgPayload.UserName} for group {msgPayload.GroupName}");

            //TODO: Save message
            await _mediator.Send(
                new StoreChatCommand
                {
                    UserId = msgPayload.UserId,
                    UserName = msgPayload.UserName,
                    GroupId = msgPayload.GroupId,
                    DateCreatedUtc = msgPayload.DateCreatedUtc,
                    Content = msgPayload.Content,
                });


            await Clients.Group(msgPayload.GroupName).SendAsync(ClientCommand.MessageToGroup, message);
        }
    }
}
