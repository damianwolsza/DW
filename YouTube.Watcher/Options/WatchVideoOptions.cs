using CommandLine;

namespace YouTube.Watcher.Options;

public class WatchVideoOptions
{
    [Option('u', "url", Required = true, HelpText = "YouTube video url")]
    public string Url { get; set; } = default!;

    [Option('t', "timeoutMin", Required = false, HelpText = "Timeout in minutes")]
    public int? TimeoutMin { get; set; }
}
