using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Dtos;
using PlatformService.Interface;
using PlatformService.Models;
using PlatformService.SyncDataServices.HttpGet;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/platform")]
public class PlatformController : ControllerBase
{

    private readonly ILogger<PlatformController> _logger;
    private readonly IPlatformRepo _context;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _client;
    private readonly IMessageBusCLient _busClient;


    public PlatformController(ILogger<PlatformController> logger,
     IPlatformRepo platform, IMapper mapper,
     ICommandDataClient client,
     IMessageBusCLient busCLient
     
     )
    {
        _logger = logger;
        _context = platform;
        _mapper = mapper;
        _client = client;
        _busClient = busCLient;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Platform>>> GetPlatforms()
    {

        return Ok(await _context.GetPlatforms());

    }


    [HttpGet("{id}", Name = "GetPlatformById")]
    public async Task<ActionResult<PlatformReadDto>> GetPlatformById(int id)
    {
        var p = await _context.GetPlatform(id);

        return p == null ? NotFound() : Ok(_mapper.Map<PlatformReadDto>(p));

    }


    [HttpPost]
    public async Task<ActionResult<Platform>> AddPlatform(PlatformCreateDto platformCreate)
    {

        var platformModel = _mapper.Map<Platform>(platformCreate);

        await _context.AddPlatform(platformModel);

        await _context.savechanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);


        //Sync
        try
        {
            await _client.SendPlatformtoCommand(platformReadDto);
            Console.WriteLine("Successfully sent in Sync");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to sent in Sync");
            Console.WriteLine(ex.Message);
        }

        //ASync

        try
        {
            Console.WriteLine("Converted into platformPublishDto");
            PlatformPublishDto message = _mapper.Map<PlatformPublishDto>(platformReadDto);

            message.Event = "Platform_Published";

            Console.WriteLine($"Sending to Rabbit MQ {message.Event}");
            _busClient.PublishnewPlatform(message);

           
            Console.WriteLine("Successfully sent in ASync");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to sent in ASync");
            Console.WriteLine(ex.Message);
            throw;
        }
        
        // try
        // {
        //      Console.WriteLine("Sending to Rabbit MQ Producer");
        //     _producer.SendMessage<string>("gg");
        //      Console.WriteLine("Successfully Sent!!!");
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine("Failed to sent in ASync Producer");
        //     Console.WriteLine(ex.Message);
            
        // }

        return platformModel;

    }

}
