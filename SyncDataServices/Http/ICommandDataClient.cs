using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.HttpGet

{
    public interface ICommandDataClient
    {
        Task SendPlatformtoCommand(PlatformReadDto p);
    }
}