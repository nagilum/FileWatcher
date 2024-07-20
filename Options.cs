namespace FileWatcher;

public class Options : IOptions
{
    /// <summary>
    /// <inheritdoc cref="IOptions.IncludeSubdirectories"/>
    /// </summary>
    public bool IncludeSubdirectories { get; set; }

    /// <summary>
    /// <inheritdoc cref="IOptions.Path"/>
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// <inheritdoc cref="IOptions.Filters"/>
    /// </summary>
    public List<string> Filters { get; } = [];
}