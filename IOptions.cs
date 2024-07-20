namespace FileWatcher;

public interface IOptions
{
    /// <summary>
    /// Whether to watch only top folder or all sub folders.
    /// </summary>
    bool IncludeSubdirectories { get; set; }
    
    /// <summary>
    /// Folder path to watch.
    /// </summary>
    string? Path { get; set; }
    
    /// <summary>
    /// File filters to watch.
    /// </summary>
    List<string> Filters { get; }
}