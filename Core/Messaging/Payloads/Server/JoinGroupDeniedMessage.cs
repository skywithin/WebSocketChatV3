namespace Core.Messaging.Payloads.Server
{
    public class JoinGroupDeniedMessage : IMessagePayload
    {
        public string GroupName { get; set; }
        public string DeniedReason { get; set; }

        public JoinGroupDeniedMessage() { } //Must have default constructor

        public JoinGroupDeniedMessage(string groupName, string deniedReason)
        {
            GroupName = groupName;
            DeniedReason = deniedReason;
        }
    }
}
