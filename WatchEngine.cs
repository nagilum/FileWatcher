namespace FileWatcher;

public class WatchEngine(
    IOptions options,
    CancellationToken cancellationToken) : IWatchEngine
{
    /// <summary>
    /// Cancellation token.
    /// </summary>
    private readonly CancellationToken _cancellationToken = cancellationToken;

    /// <summary>
    /// Console logger.
    /// </summary>
    private readonly ConsoleLogger _logger = new();
    
    /// <summary>
    /// Parsed options.
    /// </summary>
    private readonly IOptions _options = options;

    /// <summary>
    /// <inheritdoc cref="IWatchEngine.Init"/>
    /// </summary>
    public async Task Init()
    {
        var filters = string.Join(" ", _options.Filters);
        
        _logger.Information($"Watching {_options.Path!}");
        _logger.Information($"File filter(s): {filters}");
        _logger.Information($"Including subfolders: {_options.IncludeSubdirectories.ToString().ToLower()}");

        foreach (var filter in _options.Filters)
        {
            var fsw = new FileSystemWatcher(
                _options.Path!,
                filter);

            fsw.IncludeSubdirectories = _options.IncludeSubdirectories;
            fsw.EnableRaisingEvents = true;
        
            fsw.NotifyFilter = 
                NotifyFilters.Attributes |
                NotifyFilters.CreationTime |
                NotifyFilters.DirectoryName |
                NotifyFilters.FileName |
                NotifyFilters.LastAccess |
                NotifyFilters.LastWrite |
                NotifyFilters.Security |
                NotifyFilters.Size;
        
            fsw.Changed += FileSystemEventTrigger;
            fsw.Created += FileSystemEventTrigger;
            fsw.Deleted += FileSystemEventTrigger;
            fsw.Renamed += FileSystemRenamedTrigger;
        }

        var delay = TimeSpan.FromMilliseconds(10);

        while (!_cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(delay, _cancellationToken);
        }
    }

    /// <summary>
    /// Occurs when a file or directory in the specified path is changed, created, or deleted.
    /// </summary>
    private void FileSystemEventTrigger(object sender, FileSystemEventArgs e)
    {
        if (_cancellationToken.IsCancellationRequested)
        {
            return;
        }

        PathType pathType;

        try
        {
            var attr = File.GetAttributes(e.FullPath);

            pathType = attr is FileAttributes.Directory
                ? PathType.Directory
                : PathType.File;
        }
        catch
        {
            pathType = PathType.Unknown;
        }
        
        switch (e.ChangeType)
        {
            case WatcherChangeTypes.Changed:
                _logger.EventChanged(pathType, e.FullPath);
                break;
            
            case WatcherChangeTypes.Created:
                _logger.EventCreated(pathType, e.FullPath);
                break;
            
            case WatcherChangeTypes.Deleted:
                _logger.EventDeleted(pathType, e.FullPath);
                break;
        }
    }
    
    /// <summary>
    /// Occurs when a file or directory in the specified Path is renamed.
    /// </summary>
    private void FileSystemRenamedTrigger(object sender, RenamedEventArgs e)
    {
        if (_cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var oldPath = Path.GetDirectoryName(e.OldFullPath);

        string newFilePath;

        if (oldPath is null)
        {
            newFilePath = e.FullPath;
        }
        else
        {
            newFilePath = e.FullPath.StartsWith(oldPath)
                ? $".{e.FullPath[oldPath.Length..]}"
                : e.FullPath;
        }
        
        PathType pathType;

        try
        {
            var attr = File.GetAttributes(e.FullPath);

            pathType = attr is FileAttributes.Directory
                ? PathType.Directory
                : PathType.File;
        }
        catch
        {
            pathType = PathType.Unknown;
        }
        
        _logger.EventRenamed(
            pathType,
            e.OldFullPath,
            newFilePath);
    }
}