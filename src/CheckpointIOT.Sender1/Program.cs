using CheckpointIOT.Shared.Messaging;

var settings = new RabbitMqSettings();

using var connection = RabbitMqInfrastructure.CreateConnection(settings);
using var channel = connection.CreateModel();

RabbitMqInfrastructure.ConfigureTopology(channel);

Console.WriteLine("Sender 1 iniciado. Pressione ENTER para enviar frutas da epoca ou Q para sair.");
Console.WriteLine($"RabbitMQ: {settings.HostName}:{settings.Port}");

while (true)
{
	var key = Console.ReadKey(intercept: true);
	if (key.Key == ConsoleKey.Q)
	{
		break;
	}

	if (key.Key != ConsoleKey.Enter)
	{
		continue;
	}

	var message = SampleDataFactory.CreateFruitMessage();
	RabbitMqInfrastructure.PublishJson(channel, RabbitMqTopology.ValidationExchange, RabbitMqTopology.FruitValidationRoutingKey, message);

	Console.WriteLine($"[Sender1] Fruta enviada para validacao: {message.FruitName} | Solicitacao: {message.RequestedAt:dd/MM/yyyy HH:mm:ss}");
}
