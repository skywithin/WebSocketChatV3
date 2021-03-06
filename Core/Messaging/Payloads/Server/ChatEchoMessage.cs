using System;

namespace Core.Messaging.Payloads.Server
{
    public class ChatEchoMessage : IMessagePayload
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public string Content { get; set; }
        public DateTime DateCreatedUtc { get; set; }

        public ChatEchoMessage() { } //Must have default constructor

        public ChatEchoMessage(
            Guid userId, 
            string userName,
            Guid groupId,
            string groupName,
            string content,
            DateTime dateCreatedUtc)
        {
            UserId = userId;
            UserName = userName;
            GroupName = groupName;
            GroupId = groupId;
            Content = content;
            DateCreatedUtc = dateCreatedUtc;
        }
    }
}
