using System;
using RabbitMQ.Client;

namespace MicroServicesDemo.Abstractions
{
    public interface IRabbitMQConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}