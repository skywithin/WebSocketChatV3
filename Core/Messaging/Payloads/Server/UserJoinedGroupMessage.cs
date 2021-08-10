using System;

namespace Core.Messaging.Payloads
{
    public class UserJoinedGroupMessage : IMessagePayload
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }

        public UserJoinedGroupMessage() { } //Must have default constructor

        public UserJoinedGroupMessage(Guid userId, string userName, string groupName) 
        {
            UserId = userId;
            UserName = userName;
            GroupName = groupName;
        }
    }
}
