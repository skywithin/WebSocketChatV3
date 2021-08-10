using Application.Config;
using Application.MediatR.Commands.AddUserToGroup;
using Application.MediatR.Commands.FindOrCreateGroup;
using Application.Services.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IChatGroupService
    {
        Task<JoinGroupResult> JoinGroup(JoinGroupRequest request);
    }

    public class ChatGroupService : IChatGroupService
    {
        private readonly ILogger<ChatGroupService> _logger;
        private readonly IMediator _mediator;

        public ChatGroupService(
            ILogger<ChatGroupService> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<JoinGroupResult> JoinGroup(JoinGroupRequest joinRequest)
        {
            var group = await _mediator.Send(new FindOrCreateGroupCommand { GroupName = joinRequest.GroupName });

            // Check if user is already in the group
            if (group.MemberIds.Contains(joinRequest.UserId))
            {
                return new JoinGroupResult
                (
                    isAllowed: true,
                    group.GroupId,
                    group.GroupName,
                    joinRequest.UserId
                );
            }

            // Check if user is allowed to join
            if (group.MemberIds.Contains(joinRequest.UserId) == false &&
                group.MemberIds.Count() >= Constants.MAX_USERS_PER_GROUP)
            {
                //Group is full
                return new JoinGroupResult 
                (
                    isAllowed: false,
                    group.GroupId,
                    group.GroupName,
                    joinRequest.UserId,
                    deniedReason: $"Sorry, group {joinRequest.GroupName} is full" 
                );
            }

            // OK to add
            await _mediator.Send(new AddUserToGroupCommand { UserId = joinRequest.UserId, GroupId = group.GroupId });

            return new JoinGroupResult
            (
                isAllowed: true,
                group.GroupId,
                group.GroupName,
                joinRequest.UserId
            );
        }
    }
}
