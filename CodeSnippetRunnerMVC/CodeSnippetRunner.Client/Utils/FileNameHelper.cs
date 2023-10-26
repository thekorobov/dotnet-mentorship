namespace CodeSnippetRunner.Client.Utils;

public static class FileNameHelper
{
    public static string ModifyFileName()
    {
        return AppendGuidToFileName(String.Empty);
    }
    
    public static string ModifyFileName(string originalName)
    {
        const int maxLength = 20;

        return originalName.Length > maxLength ? AppendGuidToFileName(originalName) : AppendRandomChars(originalName, 3);
    }
    
    private static string AppendRandomChars(string originalName, int numberOfChars)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ012345678";
        var random = new Random();
        var randomString = new string(Enumerable.Repeat(chars, numberOfChars)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    
        var dotIndex = originalName.LastIndexOf('.');
        if (dotIndex == -1)
        {
            originalName = string.Concat(originalName, ".txt");
            dotIndex = originalName.LastIndexOf('.');
        }
    
        originalName = originalName.Insert(dotIndex, "_" + randomString);
    
        return originalName;
    }
    
    private static string AppendGuidToFileName(string originalName)
    {
        if (originalName.Contains(".cs"))
        {
            return Guid.NewGuid().ToString().Substring(0, 13) + ".cs";
        }
        return Guid.NewGuid().ToString().Substring(0, 13) + ".txt";
    }
}