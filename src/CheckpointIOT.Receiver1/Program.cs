using CheckpointIOT.Shared.Messaging;
using RabbitMQ.Client.Events;

var settings = new RabbitMqSettings();

using var connection = RabbitMqInfrastructure.CreateConnection(settings);
using var channel = connection.CreateModel();

RabbitMqInfrastructure.ConfigureTopology(channel);

Console.WriteLine("Receiver 1 iniciado. Aguardando frutas validadas.");
Console.WriteLine($"RabbitMQ: {settings.HostName}:{settings.Port}");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (_, args) =>
{
	var message = RabbitMqInfrastructure.Deserialize<FruitSeasonMessage>(args.Body);

	Console.WriteLine("[Receiver1] Mensagem recebida");
	Console.WriteLine($"  Id...........: {message.MessageId}");
	Console.WriteLine($"  Fruta........: {message.FruitName}");
	Console.WriteLine($"  Resumo.......: {message.Summary}");
	Console.WriteLine($"  Solicitacao..: {message.RequestedAt:dd/MM/yyyy HH:mm:ss zzz}");

	channel.BasicAck(args.DeliveryTag, multiple: false);
};

channel.BasicConsume(RabbitMqTopology.FruitReceiverQueue, autoAck: false, consumerTag: string.Empty, noLocal: false, exclusive: false, arguments: null, consumer: consumer);

Console.WriteLine("Pressione ENTER para encerrar.");
Console.ReadLine();
