using MediatR;
using System;

namespace Application.MediatR.Commands.RemoveUserFromGroup
{
    public class RemoveUserFromGroupCommand : IRequest<RemoveUserFromGroupCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }

        public RemoveUserFromGroupCommand(Guid userId, Guid groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }
    }
}
