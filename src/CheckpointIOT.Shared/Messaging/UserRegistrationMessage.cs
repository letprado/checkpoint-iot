namespace CheckpointIOT.Shared.Messaging;

public sealed record UserRegistrationMessage
{
    public Guid MessageId { get; init; } = Guid.NewGuid();
    public string FullName { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string Rg { get; init; } = string.Empty;
    public string Cpf { get; init; } = string.Empty;
    public DateTimeOffset RegisteredAt { get; init; }
}