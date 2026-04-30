namespace CheckpointIOT.Shared.Messaging;

public sealed class RabbitMqSettings
{
    public string HostName { get; init; } = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
    public int Port { get; init; } = int.TryParse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"), out var port) ? port : 5672;
    public string UserName { get; init; } = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
    public string Password { get; init; } = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest";
}