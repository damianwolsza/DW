using Microsoft.Playwright;

namespace YouTube.Watcher.Pages;

public static class PageAccessor
{
    private static IPage _page = default!;

    public static void Set(IPage page)
    {
        _page = page;
    }
    public static IPage Get()
        => _page;
}
