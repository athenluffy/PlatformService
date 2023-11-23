namespace PlatformService.AsyncDataServices
{
    public interface IMessageProducer
    {
        void SendMessage<T> (T message);
    }
}