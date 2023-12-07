using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Grpc;
using PlatformService.Interface;
using PlatformService.Repository;
using PlatformService.SyncDataServices.Http;
using PlatformService.SyncDataServices.HttpGet;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    //Setup a HTTP/2 endpoint without TLS.
    options.ListenAnyIP(666, o => o.Protocols =
        HttpProtocols.Http2);
        options.ListenAnyIP(777, o => o.Protocols =
        HttpProtocols.Http1);

});

//Add services to the container.
// if (builder.Environment.IsProduction())
// {
//     builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("platformdb")));
//     Console.WriteLine("Using Sql Server");
// }
// else
// {
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemory"));
    Console.WriteLine("Using InMemory Database");
//}
// builder.Services.AddHostedService<MessageBusSubscriber>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddSingleton<IMessageBusCLient, MessageBusClient>();
// builder.Services.AddSingleton<IMessageProducer,RabbitMQProducer>();

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//await SeedData.PrePopulateAsync(app, app.Environment.IsProduction());

//app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/protos/platform.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("protos/platform.proto"));
});
app.Run();
