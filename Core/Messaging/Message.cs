using System;

namespace Core.Messaging
{
    public interface IMessagePayload
    {
    }

    public class Message<TPayload> where TPayload : IMessagePayload
    {
        public DateTime DateCreatedUtc { get; set; } = DateTime.UtcNow;
        public TPayload Payload { get; set; }
    }
}
