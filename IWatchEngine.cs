namespace FileWatcher;

public interface IWatchEngine
{
    /// <summary>
    /// Setup and watch the given folder.
    /// </summary>
    Task Init();
}