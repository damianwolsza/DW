
using Microsoft.Playwright;

namespace YouTube.Watcher.Pages;

internal class BasePage : ILoadedPage
{
    protected IPage _page = PageAccessor.Get();

    public BasePage()
    {
        LoadAsync().Wait();
    }

    public async virtual Task LoadAsync()
    {
        await Task.Delay(500);
    }

    public static async Task<T> Create<T>() where T : ILoadedPage
    {
        T instance = (T)Activator.CreateInstance(typeof(T), true)!;
        await instance.LoadAsync();
        return instance;
    }

    public async Task<T> GotoAsync<T>(string url) where T : ILoadedPage
    {
        await _page.GotoAsync(url);
        await Task.Delay(200);
        return await Create<T>();
    }

    public async Task TakeScreenshot(string name = "")
    {
        string screenshotsDirectory = "screenshots";

        if (!Directory.Exists(screenshotsDirectory))
        {
            Directory.CreateDirectory(screenshotsDirectory);
        }

        // Capture screenshot and save it to the specified directory
        string screenshotPath = $"{screenshotsDirectory}/{DateTime.Now:yyyyMMdd-HHmmss}-{name}.png";
        await _page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });

    }
}
