using PlatformService.Models;

namespace PlatformService.Interface 
{
    public interface IPlatformRepo
    {
        Task<bool> savechanges();

        Task<IEnumerable<Platform>> GetPlatforms();

        Task<Platform?> GetPlatform(int id);

        Task<Platform?> AddPlatform(Platform platform);

        void Delete(Platform platform); 
    }
}