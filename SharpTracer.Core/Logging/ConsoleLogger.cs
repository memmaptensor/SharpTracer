namespace SharpTracer.Core.Logging;

public class ConsoleLogger : ILogger
{
    private static readonly ConsoleLogger _logger = new();
    private static readonly ConsoleColor _defaultColor = ConsoleColor.White;

    private ConsoleLogger()
    {
    }

    public void LogDebug(string message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{DateTime.Now} -> Debug: {message}");
    }

    public void LogInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"{DateTime.Now} -> Debug: {message}");
    }

    public void LogWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{DateTime.Now} -> Debug: {message}");
    }

    public void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{DateTime.Now} -> Debug: {message}");
    }

    public static ConsoleLogger Get() => _logger;

    public void ResetColor() => Console.ForegroundColor = _defaultColor;
}
