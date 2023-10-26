using System.Security.Cryptography;

namespace Reminders.BLL.Utils;

public static class VerificationCodeGenerateHelper
{
     public static string GenerateVerificationCode()
     {
          return Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
     }
}