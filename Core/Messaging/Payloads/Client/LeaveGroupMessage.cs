using System;

namespace Core.Messaging.Payloads
{
    public class LeaveGroupMessage : IMessagePayload
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }

        public LeaveGroupMessage() { } //Must have default constructor

        public LeaveGroupMessage(Guid userId, string userName, string groupName)
        {
            UserId = userId;
            UserName = userName;
            GroupName = groupName;
        }
    }
}
