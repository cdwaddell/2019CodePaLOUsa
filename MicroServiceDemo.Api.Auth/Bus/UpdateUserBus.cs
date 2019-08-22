using System.Text;
using MicroServiceDemo.Api.Auth.Models;
using MicroServicesDemo.Bus;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MicroServiceDemo.Api.Auth.Bus
{
    public class UpdateUserBus : IUpdateUserBus
    {
        private readonly IConnection _connection;

        public UpdateUserBus(IConnection connection)
        {
            _connection = connection;
        }

        public void SendUpdate(UserDto user)
        {
            using(var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: BusConstants.BlogQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var message = JsonConvert.SerializeObject(user);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(
                    exchange: BusConstants.EventQueueExchange,
                    routingKey: BusConstants.BlogQueueName,
                    basicProperties: properties,
                    body: body);
            }
        }
    }
}
