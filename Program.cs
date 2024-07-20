namespace FileWatcher;

internal static class Program
{
    /// <summary>
    /// Init all the things...
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    private static async Task Main(string[] args)
    {
        if (args.Length is 0 ||
            args.Any(n => n is "-h" or "--help"))
        {
            ShowProgramUsage();
            return;
        }

        if (!TryParseCmdArgs(args, out var options))
        {
            return;
        }

        var tokenSource = new CancellationTokenSource();
        var engine = new WatchEngine(options, tokenSource.Token);
        var logger = new ConsoleLogger();

        Console.CancelKeyPress += (_, e) =>
        {
            tokenSource.Cancel();
            e.Cancel = true;
            logger.Warning("Aborted by user!");
        };

        try
        {
            await engine.Init();
        }
        catch (Exception ex)
        {
            if (ex is TaskCanceledException)
            {
                return;
            }

            logger.Error(ex.Message);
        }
    }

    /// <summary>
    /// Show program usage and options.
    /// </summary>
    private static void ShowProgramUsage()
    {
        var lines = new[]
        {
            "FileWatcher v0.1-alpha",
            "Watches the given folder for file system events.",
            "",
            "Usage:",
            "  filewatcher <path> [<options>]",
            "",
            "Options:",
            "  -f|--filter <pattern>   File filter pattern to watch. Defaults to *",
            "  -s|--subs               Whether to watch top folder or all subfolders.",
            "",
            "Source and documentation available at https://github.com/nagilum/filewatcher",
            ""
        };

        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }

    /// <summary>
    /// Attempt to parse command line arguments.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <param name="options">Parsed options.</param>
    /// <returns>Success.</returns>
    private static bool TryParseCmdArgs(string[] args, out IOptions options)
    {
        options = new Options();

        var skip = false;

        for (var i = 0; i < args.Length; i++)
        {
            if (skip)
            {
                skip = false;
                continue;
            }

            var argv = args[i];

            switch (argv)
            {
                case "-f":
                case "--filter":
                    if (i == args.Length - 1)
                    {
                        Console.WriteLine($"Error: {argv} needs to be followed by a filter pattern.");
                        return false;
                    }

                    options.Filters.Add(args[i + 1]);
                    skip = true;
                    break;
                
                case "-s":
                case "--subs":
                    options.IncludeSubdirectories = true;
                    break;
                
                default:
                    if (!Directory.Exists(argv))
                    {
                        Console.WriteLine($"Error: {argv} is not a folder path.");
                        return false;
                    }

                    if (options.Path is not null)
                    {
                        Console.WriteLine("You can only watch one path.");
                        return false;
                    }

                    options.Path = argv;
                    break;
            }
        }

        if (options.Filters.Count is 0)
        {
            options.Filters.Add("*");
        }

        if (options.Path is not null)
        {
            return true;
        }

        Console.WriteLine("Error: Must provide folder path to watch.");
        return false;
    }
}