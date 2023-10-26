namespace TicketFlow.Application.Utils;

public static class NullSafeUpdateHelper
{
    public static string UpdateIfNotNullOrEmpty(string original, string newValue)
    {
        return !string.IsNullOrEmpty(newValue) ? newValue : original;
    }
}