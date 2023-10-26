namespace Reminders.BLL.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(int userId, string email, string role);
}