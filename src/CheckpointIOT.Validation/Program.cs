using CheckpointIOT.Shared.Messaging;
using RabbitMQ.Client.Events;

var settings = new RabbitMqSettings();

using var connection = RabbitMqInfrastructure.CreateConnection(settings);
using var channel = connection.CreateModel();

RabbitMqInfrastructure.ConfigureTopology(channel);
channel.BasicQos(0, 1, false);

Console.WriteLine("Validation iniciado. Aguardando mensagens para validar.");
Console.WriteLine($"RabbitMQ: {settings.HostName}:{settings.Port}");

var fruitConsumer = new EventingBasicConsumer(channel);
fruitConsumer.Received += (_, args) =>
{
	try
	{
		var message = RabbitMqInfrastructure.Deserialize<FruitSeasonMessage>(args.Body);
		if (ValidationService.TryValidateFruit(message, out var errors))
		{
			RabbitMqInfrastructure.PublishJson(channel, RabbitMqTopology.DeliveryExchange, RabbitMqTopology.FruitReceiverRoutingKey, message);
			Console.WriteLine($"[Validation] Fruta validada e enviada ao Receiver 1: {message.FruitName}");
		}
		else
		{
			Console.WriteLine($"[Validation] Fruta rejeitada: {string.Join(" | ", errors)}");
		}

		channel.BasicAck(args.DeliveryTag, multiple: false);
	}
	catch (Exception ex)
	{
		Console.WriteLine($"[Validation] Erro ao processar fruta: {ex.Message}");
		channel.BasicNack(args.DeliveryTag, multiple: false, requeue: false);
	}
};

var userConsumer = new EventingBasicConsumer(channel);
userConsumer.Received += (_, args) =>
{
	try
	{
		var message = RabbitMqInfrastructure.Deserialize<UserRegistrationMessage>(args.Body);
		if (ValidationService.TryValidateUser(message, out var errors))
		{
			RabbitMqInfrastructure.PublishJson(channel, RabbitMqTopology.DeliveryExchange, RabbitMqTopology.UserReceiverRoutingKey, message);
			Console.WriteLine($"[Validation] Usuario validado e enviado ao Receiver 2: {message.FullName}");
		}
		else
		{
			Console.WriteLine($"[Validation] Usuario rejeitado: {string.Join(" | ", errors)}");
		}

		channel.BasicAck(args.DeliveryTag, multiple: false);
	}
	catch (Exception ex)
	{
		Console.WriteLine($"[Validation] Erro ao processar usuario: {ex.Message}");
		channel.BasicNack(args.DeliveryTag, multiple: false, requeue: false);
	}
};

channel.BasicConsume(RabbitMqTopology.FruitValidationQueue, autoAck: false, consumerTag: string.Empty, noLocal: false, exclusive: false, arguments: null, consumer: fruitConsumer);
channel.BasicConsume(RabbitMqTopology.UserValidationQueue, autoAck: false, consumerTag: string.Empty, noLocal: false, exclusive: false, arguments: null, consumer: userConsumer);

Console.WriteLine("Pressione ENTER para encerrar.");
Console.ReadLine();
