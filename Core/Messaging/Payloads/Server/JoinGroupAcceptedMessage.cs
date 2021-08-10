using System;

namespace Core.Messaging.Payloads.Server
{
    public class JoinGroupAcceptedMessage : IMessagePayload
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }

        public JoinGroupAcceptedMessage() { } //Must have default constructor

        public JoinGroupAcceptedMessage(Guid groupId, string groupName)
        {
            GroupId = groupId;
            GroupName = groupName;
        }
    }
}
