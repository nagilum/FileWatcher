namespace FileWatcher;

public class ConsoleLogger : IConsoleLogger
{
    /// <summary>
    /// Lock, to ensure entire event gets written to console.
    /// </summary>
    private readonly object _padLock = new();

    /// <summary>
    /// <inheritdoc cref="IConsoleLogger.EventChanged"/>
    /// </summary>
    public void EventChanged(PathType pathType, string filePath)
    {
        var type = pathType switch
        {
            PathType.Directory => "Folder",
            PathType.File => "File",
            PathType.Unknown => string.Empty,
            _ => string.Empty
        };

        this.LogToConsole(
            ConsoleColor.DarkCyan,
            $"{type}Changed",
            filePath);
    }

    /// <summary>
    /// <inheritdoc cref="IConsoleLogger.EventCreated"/>
    /// </summary>
    public void EventCreated(PathType pathType, string filePath)
    {
        var type = pathType switch
        {
            PathType.Directory => "Folder",
            PathType.File => "File",
            PathType.Unknown => string.Empty,
            _ => string.Empty
        };

        this.LogToConsole(
            ConsoleColor.DarkGreen,
            $"{type}Created",
            filePath);
    }

    /// <summary>
    /// <inheritdoc cref="IConsoleLogger.EventDeleted"/>
    /// </summary>
    public void EventDeleted(PathType pathType, string filePath)
    {
        var type = pathType switch
        {
            PathType.Directory => "Folder",
            PathType.File => "File",
            PathType.Unknown => string.Empty,
            _ => string.Empty
        };

        this.LogToConsole(
            ConsoleColor.DarkMagenta,
            $"{type}Deleted",
            filePath);
    }

    /// <summary>
    /// <inheritdoc cref="IConsoleLogger.EventRenamed"/>
    /// </summary>
    public void EventRenamed(PathType pathType, string oldFilePath, string newFilePath)
    {
        var type = pathType switch
        {
            PathType.Directory => "Folder",
            PathType.File => "File",
            PathType.Unknown => string.Empty,
            _ => string.Empty
        };

        this.LogToConsole(
            ConsoleColor.DarkBlue,
            $"{type}Renamed",
            $"{oldFilePath} to {newFilePath}");
    }

    /// <summary>
    /// <inheritdoc cref="IConsoleLogger.Error"/>
    /// </summary>
    public void Error(string message)
        => this.LogToConsole(ConsoleColor.DarkRed, "Error", message);

    /// <summary>
    /// <inheritdoc cref="IConsoleLogger.Information"/>
    /// </summary>
    public void Information(string message)
        => this.LogToConsole(ConsoleColor.White, "Information", message);

    /// <summary>
    /// <inheritdoc cref="IConsoleLogger.Warning"/>
    /// </summary>
    public void Warning(string message)
        => this.LogToConsole(ConsoleColor.DarkYellow, "Warning", message);

    /// <summary>
    /// Log to console.
    /// </summary>
    /// <param name="color">Color for type.</param>
    /// <param name="type">Type of message.</param>
    /// <param name="message">Message to write.</param>
    private void LogToConsole(
        ConsoleColor color,
        string type,
        string message)
    {
        lock (_padLock)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");

            Console.ForegroundColor = color;
            Console.Write($"[{type}] ");
        
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }
}