using System;
using System.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Models;
using MicroServiceDemo.Api.Comments.Abstractions;
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
        private readonly IServiceProvider _provider;
        private readonly IModel _channel;

        public UserUpdateReceiver(IConnection connection, IServiceProvider provider)
        {
            _connection = connection;
            _provider = provider;
            _channel = _connection.CreateModel();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _channel.QueueDeclare(
                queue: BusConstants.CommentQueueName,
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

                    var context = provider.GetService<IBlogDbContext>();
                    var user = context.Users.SingleOrDefault(u => u.Username == dto.Username);

                    if (user != null)
                    {
                        user.Bio = dto.Bio;
                        user.Image = dto.Image;
                        context.SaveChanges();
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
