using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using PlatformService.SyncDataServices.HttpGet;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpclient;
        private readonly IConfiguration _config;

        public HttpCommandDataClient(HttpClient httpClient,IConfiguration configuration)
        {
            _httpclient = httpClient;
            _config = configuration;

        }
        public async Task SendPlatformtoCommand(PlatformReadDto p)
        {
            var req = new StringContent( JsonSerializer.Serialize(p),Encoding.UTF8,"application/json");

            var response = await _httpclient.PostAsync($"{_config["CommandService"]}/api/command",req);

            if(response.IsSuccessStatusCode)
            
                Console.WriteLine("Added Command to Command Service XD");
            
            else
            {
                 Console.WriteLine("Failed to connect to Command Service ^-^ ");
            }
            

            
        
        }



    }
}