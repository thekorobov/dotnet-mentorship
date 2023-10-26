namespace Reminders.UnitTests;

public static class Constants
{
    // User constants
    public const string ValidEmail = "test@gmail.com";
    public const string ValidUserName = "testUser";
    public const string ValidPassword = "Test12345!";
    public const int ValidId = 1;
    public const int ValidUserId = 1;
    public const int ValidCurrentUserId = 1;
    public const int NonExistingUserId = 999;
    public const int InvalidUserId = -1;
    public const AuthProviderType ValidAuthProvider = AuthProviderType.SimpleAuth;

    // Reminder constants
    public const int ExistingReminderId1 = 4;
    public const int ExistingReminderId2 = 5;
    public const int NonExistingReminderId = 100;
    public const string FirstValidReminderName = "Reminder name 1";
    public const string SecondValidReminderName = "Reminder name 2";
    public static readonly DateTime ValidDate = DateTime.Now.AddDays(1);

    // Verification constants
    public const string ValidVerificationToken1 = "DASLJKL54345";
    public const string ValidVerificationToken2 = "HFASDD7823H";
    public const string ValidUpdateVerificationToken = "DASDASFGGGG1323145";
}