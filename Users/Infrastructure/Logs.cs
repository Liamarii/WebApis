using System.Runtime.CompilerServices;

namespace Users.Infrastructure;

public sealed class Logs
{
    private static readonly Lazy<Logs> _instance = new(() => new Logs());
    private readonly ILogger _logger;

    private Logs() {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _logger = LoggerFactory
            .Create(x => x.AddDebug())
            .CreateLogger<Logs>();
    }

    public static Logs Add => _instance.Value;

    public void InfoLog(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "")
    {
        Log(LogLevel.Information, message, callerName, filePath);
    }

    public void WarningLog(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "")
    {
        Log(LogLevel.Warning, message, callerName, filePath);
    }

    public void ErrorLog(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "")
    {
        Log(LogLevel.Error, message, callerName, filePath);
    }

    public void DebugLog(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "")
    {
        Log(LogLevel.Debug, message, callerName, filePath);
    }

    private void Log(LogLevel logLevel, string message, string callerName, string filePath)
    {
        _logger.Log(logLevel, "\n {logLevel} of \"{message}\" was logged by the {callerName} method in {filePath} \n", logLevel, message, callerName, filePath);
    }
}