using Microsoft.Playwright;

namespace YouTube.Watcher.Pages;

internal class YouTubeVideoPage : BasePage
{
    private ILocator CookieAcceptButtonLocator => _page.Locator("[aria-label=\"Accept the use of cookies and other data for the purposes described\"]");
    private ILocator VideoPlayButtonLocator => _page.Locator("[aria-label=\"Play\"]");
    private ILocator SkipAdsButtonLocator => _page.Locator("button.ytp-ad-skip-button-modern");
    private ILocator AdsLabelLocator => _page.Locator(".ytp-ad-simple-ad-badge");

    public override async Task LoadAsync()
    {
        await Task.Delay(10000);
        //await CookieAcceptButtonLocator.WaitForAsync(); 
    }

    public async Task AcceptCookies()
    {
        await Console.Out.WriteLineAsync("Accepting cookies");
        await TakeScreenshot("accept-cookies");
        if(await CookieAcceptButtonLocator.IsVisibleAsync())
            await CookieAcceptButtonLocator.ClickAsync();
    }

    public async Task PlayVideo(int timeoutMin, bool skipAds = true)
    {
        var lastScreenshotTime = DateTime.Now;
        var startTime = DateTime.Now;

        System.Console.WriteLine($"Watching video, timeout {timeoutMin}. Timestamp: {startTime}");

        while (startTime.AddMinutes(timeoutMin) > DateTime.Now)
        {
            if (await IsRunningAds())
            {
                if (skipAds)
                {
                    await Console.Out.WriteLineAsync("Skipping ads");
                    if (await SkipAdsButtonLocator.IsVisibleAsync())
                    {
                        try
                        {
                            await SkipAdsButtonLocator.ClickAsync();
                        }
                        catch { }
                    }
                }
                else
                {
                    await Task.Delay(10000);
                }
            }
            else if (await IsPlayButtonVisible())
            {
                await VideoPlayButtonLocator.WaitForAsync();
                await Console.Out.WriteLineAsync("Playing video");
                await VideoPlayButtonLocator.ClickAsync();
            }

            // Take a screenshot every 10 minutes
            if ((DateTime.Now - lastScreenshotTime).TotalMinutes >= 10)
            {
                await TakeScreenshot("playing-video");
                lastScreenshotTime = DateTime.Now;
            }

            await Task.Delay(5000);
        }

        await Console.Out.WriteLineAsync($"Finished watching {DateTime.Now}");
    }

    private async Task<bool> IsPlayButtonVisible()
    {
        var isPlayButtonVisible = await VideoPlayButtonLocator.IsVisibleAsync();
        Console.WriteLine($"Play button visible: {isPlayButtonVisible}");
        return isPlayButtonVisible;
    }

    private async Task<bool> IsRunningAds()
    {
        var isRunningAds = await AdsLabelLocator.IsVisibleAsync();
        Console.WriteLine($"Is running ads: {isRunningAds}");
        return isRunningAds;
    }
}
