using CheckpointIOT.Shared.Messaging;
using RabbitMQ.Client.Events;

var settings = new RabbitMqSettings();

using var connection = RabbitMqInfrastructure.CreateConnection(settings);
using var channel = connection.CreateModel();

RabbitMqInfrastructure.ConfigureTopology(channel);

Console.WriteLine("Receiver 2 iniciado. Aguardando usuarios validados.");
Console.WriteLine($"RabbitMQ: {settings.HostName}:{settings.Port}");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (_, args) =>
{
	var message = RabbitMqInfrastructure.Deserialize<UserRegistrationMessage>(args.Body);

	Console.WriteLine("[Receiver2] Mensagem recebida");
	Console.WriteLine($"  Id...........: {message.MessageId}");
	Console.WriteLine($"  Nome.........: {message.FullName}");
	Console.WriteLine($"  Endereco.....: {message.Address}");
	Console.WriteLine($"  RG...........: {message.Rg}");
	Console.WriteLine($"  CPF..........: {message.Cpf}");
	Console.WriteLine($"  Registro.....: {message.RegisteredAt:dd/MM/yyyy HH:mm:ss zzz}");

	channel.BasicAck(args.DeliveryTag, multiple: false);
};

channel.BasicConsume(RabbitMqTopology.UserReceiverQueue, autoAck: false, consumerTag: string.Empty, noLocal: false, exclusive: false, arguments: null, consumer: consumer);

Console.WriteLine("Pressione ENTER para encerrar.");
Console.ReadLine();
