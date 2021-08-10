namespace Core.Messaging.Payloads.Server
{
    public class UserLeftGroupMessage : IMessagePayload
    {
        public string UserName { get; set; }
        public string GroupName { get; set; }

        public UserLeftGroupMessage() { } //Must have default constructor

        public UserLeftGroupMessage(string userName, string groupName)
        {
            UserName = userName;
            GroupName = groupName;
        }
    }
}
