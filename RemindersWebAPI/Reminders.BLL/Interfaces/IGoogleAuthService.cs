namespace Reminders.BLL.Interfaces;

public interface IGoogleAuthService
{
    Task<string> HandleGoogleLoginAsync(string email, string name);
}