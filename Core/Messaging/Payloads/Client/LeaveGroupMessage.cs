using System;

namespace Core.Messaging.Payloads.Client
{
    public class LeaveGroupMessage : IMessagePayload
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }

        public LeaveGroupMessage() { } //Must have default constructor

        public LeaveGroupMessage(Guid userId, string userName, Guid groupId, string groupName)
        {
            UserId = userId;
            UserName = userName;
            GroupId = groupId;
            GroupName = groupName;
        }
    }
}
