namespace CheckpointIOT.Shared.Messaging;

public sealed record FruitSeasonMessage
{
    public Guid MessageId { get; init; } = Guid.NewGuid();
    public string FruitName { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
    public DateTimeOffset RequestedAt { get; init; }
}