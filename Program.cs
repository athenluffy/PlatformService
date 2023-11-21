using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Interface;
using PlatformService.Repository;
using PlatformService.SyncDataServices.Http;
using PlatformService.SyncDataServices.HttpGet;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("platformdb")));
    Console.WriteLine("Using Sql Server");
}
else
{
     builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemory"));
    Console.WriteLine("Using InMemory Database");
}

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddTransient<IMessageBusCLient, MessageBusClient>();

builder.Services.AddScoped<IPlatformRepo,PlatformRepo>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

await SeedData.PrePopulateAsync(app,app.Environment.IsProduction());

app.UseAuthorization();

app.MapControllers();

app.Run();
