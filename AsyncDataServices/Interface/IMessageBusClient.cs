using PlatformService.Dtos;

namespace PlatformService.AsyncDataServices
{
    public interface IMessageBusCLient
    {
        void PublishnewPlatform(PlatformPublishDto publishDto);
    }
}