using CommandLine;
using Microsoft.Playwright;
using YouTube.Watcher.Options;
using YouTube.Watcher.Pages;


var headless = Environment.OSVersion.Platform == PlatformID.Unix;

await Parser.Default
    .ParseArguments<WatchVideoOptions>(args)
    .MapResult(
    (WatchVideoOptions o) => WatchVideo(o),
    errs => { return Task.CompletedTask; });

async Task WatchVideo(WatchVideoOptions o)
{
    await Console.Out.WriteLineAsync("Url: " + o.Url);
    using var playwright = await Playwright.CreateAsync();

    await using var browser = await playwright.Firefox.LaunchAsync(new()
    {
        Headless = headless,
        SlowMo = 500,
    });

    var page = await browser.NewPageAsync();
    PageAccessor.Set(page);
    await page.GotoAsync(o.Url);

    var videoPlayerPage = new YouTubeVideoPage();

    await videoPlayerPage.LoadAsync();
    await videoPlayerPage.AcceptCookies();
    await videoPlayerPage.PlayVideo(o.TimeoutMin ?? 50);

}