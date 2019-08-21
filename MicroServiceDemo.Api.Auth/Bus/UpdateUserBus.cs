using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MicroServiceDemo.Api.Auth.Controllers.API.V1;
using MicroServiceDemo.Api.Auth.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MicroServiceDemo.Api.Auth.Bus
{
    public class UpdateUserBus
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
                channel.QueueDeclare(queue: "task_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var message = JsonConvert.SerializeObject(user);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                    routingKey: "task_queue",
                    basicProperties: properties,
                    body: body);
            }
        }
    }
}
