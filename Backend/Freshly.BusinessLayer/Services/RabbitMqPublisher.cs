using System.Text;
using System.Text.Json;
using Freshly.BusinessLayer.Interfaces;
using Freshly.DomainLayer.Messages;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Freshly.BusinessLayer.Services;

public class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
{
    private IConnection _connection = null!;
    private readonly IConfiguration _config = null!;
    private IChannel _channel = null!;
    private const string QueueName = "ocr-queue";

    public RabbitMqPublisher(IConfiguration config)
    {
        _config = config;
    }

    public async Task InitializeAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _config["RabbitMq:Host"],
            UserName = _config["RabbitMq:Username"],
            Password = _config["RabbitMq:Password"],
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
            );
    }

    public async Task PublishOcrJobAsync(OcrJobMessage message)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: QueueName,
            body: body
            );
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
        await _channel.DisposeAsync();
    }
}