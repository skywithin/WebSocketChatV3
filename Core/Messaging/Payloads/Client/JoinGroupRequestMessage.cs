using System;

namespace Core.Messaging.Payloads
{
    public class JoinGroupRequestMessage : IMessagePayload
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }

        public JoinGroupRequestMessage() { } //Must have default constructor

        public JoinGroupRequestMessage(
            Guid userId,
            string userName,
            string groupName)
        {
            UserId = userId;
            UserName = userName;
            GroupName = groupName;
        }
    }
}
