using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class SeedData
    {
        public static async Task PrePopulateAsync(IApplicationBuilder app,bool is_prod)
        {
            using var service_scoped = app.ApplicationServices.CreateScope();
            await PrePopulateDataAsync(service_scoped.ServiceProvider.GetService<AppDbContext>()!,is_prod);
        }

        private static async Task PrePopulateDataAsync(AppDbContext context,bool is_prod)
        {
            if(is_prod)
            {
                await context.Database.MigrateAsync();
            }

            Console.WriteLine(" ---> PrePopulating Data ...");
            if (context.Platforms.Any())
            {
                Console.WriteLine(" ---> Data Already PrePopulated ...");
                return;

            }
            await context.Platforms.AddRangeAsync(
                new Platform() { Name="Windows",Publisher="Microsoft",Cost="paid" },
                new Platform() { Name="AppleOS",Publisher="Apple",Cost="free" },
                new Platform() { Name="Android",Publisher="Google",Cost="Free" }
            );

            await context.SaveChangesAsync();


            await context.SaveChangesAsync();



        }
    }
}