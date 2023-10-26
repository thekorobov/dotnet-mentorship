using System.Text;
using System.Text.RegularExpressions;

namespace TicketFlow.Application.Utils.Generators;

public class UsernameGenerator
{
    private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    public static string GenerateUsername(string username)
    {
        var rand = new Random();
        var generateUsername = new StringBuilder($"{Regex.Replace(username, @"[^a-zA-Z0-9]", "")}_");
        for (var i = 0; i < rand.Next(6, 6); i++)
        {
            generateUsername.Append(AllowedChars[rand.Next(AllowedChars.Length)]);
        }
        return generateUsername.ToString();
    }
}