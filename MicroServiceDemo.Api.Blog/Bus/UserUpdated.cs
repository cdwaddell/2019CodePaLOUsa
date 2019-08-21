using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroServiceDemo.Api.Blog.Bus
{
    public class UserUpdated : IUserUpdated
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public UserUpdated(IConnection connection)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
        }

        public void ReceiveUpdate(UserDto user)
        {
            _channel.QueueDeclare(queue: "UserQueue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                int dots = message.Split('.').Length - 1;
                Thread.Sleep(dots * 1000);
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: "UserQueue",
                autoAck: false,
                consumer: consumer);
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
