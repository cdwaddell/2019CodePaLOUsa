using System;
using System.Linq;
using MicroServiceDemo.Api.Blog.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Auth.Bus;
using MicroServiceDemo.Api.Blog.Abstractions;
using MicroServiceDemo.Api.Blog.Mapping;
using MicroServicesDemo.Bus;
using MicroServicesDemo.DataContracts;
using MicroServicesDemo.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MicroServiceDemo.Api.Blog.Bus
{
    public class UserUpdateReceiver : IHostedService
    {
        private readonly IConnection _connection;
        private readonly IUpdateUserBus _bus;
        private readonly IServiceProvider _provider;
        private readonly IModel _channel;

        public UserUpdateReceiver(IConnection connection, IUpdateUserBus bus, IServiceProvider provider)
        {
            _connection = connection;
            _bus = bus;
            _provider = provider;
            _channel = _connection.CreateModel();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _channel.QueueDeclare(
                queue: BusConstants.BlogQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var scopeFactory = _provider.GetService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    var provider = scope.ServiceProvider;

                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var dto = message.ToTransportUser<UserDto>();

                    _bus.SendUpdate(dto);

                    var context = provider.GetService<IBlogDbContext>();
                    var user = context.Users.SingleOrDefault(u => u.Username == dto.Username);

                    if (user != null)
                    {
                        user.Bio = dto.Bio;
                        user.Image = dto.Image;
                    }

                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };

            _channel.BasicConsume(queue: BusConstants.BlogQueueName,
                autoAck: false,
                consumer: consumer);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Close();
            _channel.Dispose();
        }
    }
}
