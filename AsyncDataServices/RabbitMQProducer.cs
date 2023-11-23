
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class RabbitMQProducer(IConfiguration configuration) : IMessageProducer
    {
        private readonly IConfiguration _config = configuration;

        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory { HostName = _config["RabbitMqHost"],
                Port = int.Parse(_config["RabbitMqPort"]!)};
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "trigger", ExchangeType.Fanout);

                Console.WriteLine("Rabbit MQ Connected");
            
            var body = Encoding.UTF8.GetBytes("{\"name\": \"AndroidOS\",\"Event\": \"Google\",\"cost\": \"Free\"}");
            channel.BasicPublish(exchange: "trigger", routingKey: "", body: body);

        }
    }
}