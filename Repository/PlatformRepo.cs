using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Interface;
using PlatformService.Models;

namespace PlatformService.Repository

{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;
        public PlatformRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Platform?> AddPlatform(Platform platform)
        {
            await _context.Platforms.AddAsync(platform);

            return await _context.Platforms.FindAsync(platform.Id)!;
        }

        public void Delete(Platform platform)
        {
            throw new NotImplementedException();
        }

        public async Task<Platform?> GetPlatform(int id)
        {
            return await _context.Platforms.FindAsync(id);
        }

        public async Task<IEnumerable<Platform>> GetPlatforms()
        {
            return await _context.Platforms.ToListAsync();
        }

        public async Task<bool> savechanges()
        {
            return await _context.SaveChangesAsync()>=1;
        }
    }
}