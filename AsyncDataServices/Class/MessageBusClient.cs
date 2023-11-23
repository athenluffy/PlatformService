using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusCLient
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _config = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMqHost"],
                Port = int.Parse(_config["RabbitMqPort"])
            };
            Console.WriteLine("Constructor Called");
            try
            {
                _connection = factory.CreateConnection();

                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", ExchangeType.Fanout);

                Console.WriteLine("Rabbit MQ Connected");
                _connection.ConnectionShutdown += RabbitMqConnectionShutdown;



            }
            catch (Exception e)
            {
                Console.WriteLine("Error in  MQ Connection");
                Console.WriteLine(e.Message);
                throw;
            }

        }

        private void RabbitMqConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Rabbit MQ shutdown");
        }

        public void PublishnewPlatform(PlatformPublishDto publishDto)
        {
            Console.WriteLine(" Preparing  Message ---> ");


            if (_connection.IsOpen)
            {
                var message = JsonSerializer.Serialize(publishDto);
                Console.WriteLine("Connection Opened!!! Sending Message ---> ");

                SendMessage(message);
            }
            else
            {
                Console.WriteLine("Connection Closed!!! Could not Send Message ---> ");

            }
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", null, body);

            Console.WriteLine("Succesfully Published");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            
        }
    }
}