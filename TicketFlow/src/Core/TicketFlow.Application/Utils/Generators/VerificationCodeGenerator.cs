using System.Security.Cryptography;

namespace TicketFlow.Application.Utils.Generators;

public static class VerificationCodeGenerator
{
    public static string GenerateVerificationCode()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
    }
}