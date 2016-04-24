using Seven.Messages.QueueMessages;

namespace Seven.Messages
{
    public interface ICommunicateChannel
    {
        void Send(SendMessage message);

        ReceiveMessage Receive();

        void BasicAck(ulong deliveryTag);
    }
}