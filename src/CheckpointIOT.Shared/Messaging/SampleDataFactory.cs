namespace CheckpointIOT.Shared.Messaging;

public static class SampleDataFactory
{
    private static readonly FruitSeasonMessage[] FruitSamples =
    [
        new()
        {
            FruitName = "Manga Palmer",
            Summary = "Fruta tropical de polpa macia, doce e rica em vitaminas A e C.",
            RequestedAt = DateTimeOffset.Now
        },
        new()
        {
            FruitName = "Morango",
            Summary = "Fruta vermelha aromatica, levemente acida e muito usada em sobremesas.",
            RequestedAt = DateTimeOffset.Now
        },
        new()
        {
            FruitName = "Abacaxi",
            Summary = "Fruta refrescante de sabor agridoce, muito consumida em sucos e sobremesas.",
            RequestedAt = DateTimeOffset.Now
        }
    ];

    private static readonly UserRegistrationMessage[] UserSamples =
    [
        new()
        {
            FullName = "Ana Paula Nogueira",
            Address = "Rua das Jabuticabeiras, 150 - Sao Paulo/SP",
            Rg = "423456781",
            Cpf = "12345678909",
            RegisteredAt = DateTimeOffset.Now
        },
        new()
        {
            FullName = "Carlos Eduardo Lima",
            Address = "Avenida dos Pomares, 88 - Campinas/SP",
            Rg = "398765432",
            Cpf = "98765432100",
            RegisteredAt = DateTimeOffset.Now
        },
        new()
        {
            FullName = "Mariana Souza Rocha",
            Address = "Alameda das Hortas, 245 - Santos/SP",
            Rg = "512349876",
            Cpf = "45678912377",
            RegisteredAt = DateTimeOffset.Now
        }
    ];

    public static FruitSeasonMessage CreateFruitMessage()
    {
        var sample = FruitSamples[Random.Shared.Next(FruitSamples.Length)];
        return sample with { RequestedAt = DateTimeOffset.Now, MessageId = Guid.NewGuid() };
    }

    public static UserRegistrationMessage CreateUserMessage()
    {
        var sample = UserSamples[Random.Shared.Next(UserSamples.Length)];
        return sample with { RegisteredAt = DateTimeOffset.Now, MessageId = Guid.NewGuid() };
    }
}