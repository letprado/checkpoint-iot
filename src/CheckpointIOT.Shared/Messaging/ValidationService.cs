using System.Text.RegularExpressions;

namespace CheckpointIOT.Shared.Messaging;

public static class ValidationService
{
    public static bool TryValidateFruit(FruitSeasonMessage message, out string[] errors)
    {
        var validationErrors = new List<string>();

        if (string.IsNullOrWhiteSpace(message.FruitName))
        {
            validationErrors.Add("O nome da fruta deve ser informado.");
        }

        if (string.IsNullOrWhiteSpace(message.Summary) || message.Summary.Length < 15)
        {
            validationErrors.Add("O resumo da fruta deve conter pelo menos 15 caracteres.");
        }

        if (message.RequestedAt == default)
        {
            validationErrors.Add("A data e horario da solicitacao devem ser informados.");
        }

        errors = validationErrors.ToArray();
        return errors.Length == 0;
    }

    public static bool TryValidateUser(UserRegistrationMessage message, out string[] errors)
    {
        var validationErrors = new List<string>();

        if (string.IsNullOrWhiteSpace(message.FullName) || message.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length < 2)
        {
            validationErrors.Add("O nome completo do usuario deve conter ao menos nome e sobrenome.");
        }

        if (string.IsNullOrWhiteSpace(message.Address) || message.Address.Length < 10)
        {
            validationErrors.Add("O endereco do usuario deve ser informado.");
        }

        if (!Regex.IsMatch(message.Rg ?? string.Empty, "^[0-9]{8,10}$"))
        {
            validationErrors.Add("O RG deve conter entre 8 e 10 digitos numericos.");
        }

        if (!Regex.IsMatch(message.Cpf ?? string.Empty, "^[0-9]{11}$"))
        {
            validationErrors.Add("O CPF deve conter 11 digitos numericos.");
        }

        if (message.RegisteredAt == default)
        {
            validationErrors.Add("A data e horario do cadastro devem ser informados.");
        }

        errors = validationErrors.ToArray();
        return errors.Length == 0;
    }
}