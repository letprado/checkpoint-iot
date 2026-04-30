using CheckpointIOT.Shared.Messaging;

var settings = new RabbitMqSettings();

using var connection = RabbitMqInfrastructure.CreateConnection(settings);
using var channel = connection.CreateModel();

RabbitMqInfrastructure.ConfigureTopology(channel);

Console.WriteLine("Sender 2 iniciado. Pressione ENTER para enviar usuarios ficticios ou Q para sair.");
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

	var message = SampleDataFactory.CreateUserMessage();
	RabbitMqInfrastructure.PublishJson(channel, RabbitMqTopology.ValidationExchange, RabbitMqTopology.UserValidationRoutingKey, message);

	Console.WriteLine($"[Sender2] Usuario enviado para validacao: {message.FullName} | Registro: {message.RegisteredAt:dd/MM/yyyy HH:mm:ss}");
}
