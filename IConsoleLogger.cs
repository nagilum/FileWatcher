namespace FileWatcher;

public interface IConsoleLogger
{
    /// <summary>
    /// Log a changed file event to console.
    /// </summary>
    /// <param name="pathType">Type of path.</param>
    /// <param name="filePath">Path to file that was changed.</param>
    void EventChanged(PathType pathType, string filePath);

    /// <summary>
    /// Log a created file event to console.
    /// </summary>
    /// <param name="pathType">Type of path.</param>
    /// <param name="filePath">Path to file that was created.</param>
    void EventCreated(PathType pathType, string filePath);

    /// <summary>
    /// Log a deleted file event to console.
    /// </summary>
    /// <param name="pathType">Type of path.</param>
    /// <param name="filePath">Path to file what was deleted.</param>
    void EventDeleted(PathType pathType, string filePath);

    /// <summary>
    /// Log a renamed file event to console.
    /// </summary>
    /// <param name="pathType">Type of path.</param>
    /// <param name="oldFilePath">Old file path.</param>
    /// <param name="newFilePath">New file path.</param>
    void EventRenamed(PathType pathType, string oldFilePath, string newFilePath);
    
    /// <summary>
    /// Log an error to console.
    /// </summary>
    /// <param name="message">Error message.</param>
    void Error(string message);
    
    /// <summary>
    /// Log an info message to console.
    /// </summary>
    /// <param name="message">Info message.</param>
    void Information(string message);

    /// <summary>
    /// Log a warning message to console.
    /// </summary>
    /// <param name="message">Warning message.</param>
    void Warning(string message);
}