using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace CheckpointIOT.Shared.Messaging;

public static class RabbitMqInfrastructure
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    public static IConnection CreateConnection(RabbitMqSettings settings)
    {
        var factory = new ConnectionFactory
        {
            HostName = settings.HostName,
            Port = settings.Port,
            UserName = settings.UserName,
            Password = settings.Password
        };

        return factory.CreateConnection();
    }

    public static void ConfigureTopology(IModel channel)
    {
        channel.ExchangeDeclare(RabbitMqTopology.ValidationExchange, ExchangeType.Topic, durable: true, autoDelete: false);
        channel.ExchangeDeclare(RabbitMqTopology.DeliveryExchange, ExchangeType.Topic, durable: true, autoDelete: false);

        channel.QueueDeclare(RabbitMqTopology.FruitValidationQueue, durable: true, exclusive: false, autoDelete: false);
        channel.QueueDeclare(RabbitMqTopology.UserValidationQueue, durable: true, exclusive: false, autoDelete: false);
        channel.QueueDeclare(RabbitMqTopology.FruitReceiverQueue, durable: true, exclusive: false, autoDelete: false);
        channel.QueueDeclare(RabbitMqTopology.UserReceiverQueue, durable: true, exclusive: false, autoDelete: false);

        channel.QueueBind(RabbitMqTopology.FruitValidationQueue, RabbitMqTopology.ValidationExchange, RabbitMqTopology.FruitValidationRoutingKey);
        channel.QueueBind(RabbitMqTopology.UserValidationQueue, RabbitMqTopology.ValidationExchange, RabbitMqTopology.UserValidationRoutingKey);
        channel.QueueBind(RabbitMqTopology.FruitReceiverQueue, RabbitMqTopology.DeliveryExchange, RabbitMqTopology.FruitReceiverRoutingKey);
        channel.QueueBind(RabbitMqTopology.UserReceiverQueue, RabbitMqTopology.DeliveryExchange, RabbitMqTopology.UserReceiverRoutingKey);
    }

    public static void PublishJson<T>(IModel channel, string exchange, string routingKey, T payload)
    {
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload, SerializerOptions));
        channel.BasicPublish(exchange, routingKey, properties, body);
    }

    public static T Deserialize<T>(ReadOnlyMemory<byte> body)
    {
        var json = Encoding.UTF8.GetString(body.ToArray());
        return JsonSerializer.Deserialize<T>(json, SerializerOptions)
            ?? throw new InvalidOperationException($"Could not deserialize message to {typeof(T).Name}.");
    }
}