namespace Core.Messaging.Payloads.Client
{
    public class LoginRequestMessage : IMessagePayload
    {
        public string UserName { get; set; }

        public LoginRequestMessage() { } //Must have default constructor

        public LoginRequestMessage(string userName)
        {
            UserName = userName;
        }
    }
}
