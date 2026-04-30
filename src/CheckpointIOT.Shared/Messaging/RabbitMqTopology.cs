namespace CheckpointIOT.Shared.Messaging;

public static class RabbitMqTopology
{
    public const string ValidationExchange = "hortifruti.validation.topic";
    public const string DeliveryExchange = "hortifruti.delivery.topic";

    public const string FruitValidationQueue = "queue.validation.frutas.epoca";
    public const string UserValidationQueue = "queue.validation.usuarios.cadastro";
    public const string FruitReceiverQueue = "queue.receiver.frutas.epoca";
    public const string UserReceiverQueue = "queue.receiver.usuarios.cadastro";

    public const string FruitValidationRoutingKey = "validation.frutas.epoca";
    public const string UserValidationRoutingKey = "validation.usuarios.cadastro";
    public const string FruitReceiverRoutingKey = "receiver.frutas.epoca";
    public const string UserReceiverRoutingKey = "receiver.usuarios.cadastro";
}