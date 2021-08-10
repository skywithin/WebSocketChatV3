using Core.Messaging.Payloads;

namespace Core.Messaging
{
    public class MessageBuilder<TPayload> where TPayload : IMessagePayload
    {
        private TPayload messagePayload;

        public MessageBuilder<TPayload> WithPayload(TPayload payload)
        {
            messagePayload = payload;

            return this;
        }

        public Message<TPayload> Build()
        {
            return new Message<TPayload>
            {
                Payload = messagePayload
            };
        }
    }
}
