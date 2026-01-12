using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIPrometheusJobSearchTest.Pages
{

    public class GoogleSearchPage
    {
        private readonly IPage _page;
        private readonly ILogger _logger;

        private ILocator SearchBox => _page.Locator("textarea[name='q']");
        private ILocator SearchResults => _page.Locator("#search");
        //private ILocator PrometheusSpan => _page.Locator("span.VuuXrf", new() { HasText = "Prometheus Group" });
        private ILocator PrometheusSpan => _page.GetByText("Prometheus Group", new() { Exact = true }).First;
        private ILocator FirstSearchCareersLink => _page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Careers" }).First;

        public GoogleSearchPage(IPage page, ILogger logger)
        {
            _page = page;
            _logger = logger;
        }

        public async Task NavigateAsync(string url)
        {
            await _page.GotoAsync(url);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            _logger.LogInformation($"Navigation to {url} complete");
        }

                
        public async Task SearchForAsync(string searchTerm)
        {
            _logger.LogInformation($"Searching for: {searchTerm}");

            // Small random thinking time before interaction
            // trying to mimic human behavior and avoid bot detection aka catcha
            await RandomDelayAsync(600, 1200);

            // Move mouse toward search box (human behavior)
            var box = await SearchBox.BoundingBoxAsync();
            await _page.Mouse.MoveAsync(
                box.X + box.Width / 2,
                box.Y + box.Height / 2,
                new() { Steps = 25 }
            );

            await RandomDelayAsync(200, 500);

            // Click instead of Fill
            await SearchBox.ClickAsync();

            // Type like a human
            foreach (char c in searchTerm)
            {
                await SearchBox.TypeAsync(c.ToString());
                await RandomDelayAsync(180, 200);
            }

            await RandomDelayAsync(500, 1000);

            await _page.Keyboard.PressAsync("Enter");

            // Wait for results in a human-like way
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await RandomDelayAsync(1000, 2000);
        }
        private async Task RandomDelayAsync(int minMs, int maxMs)
        {
            var delay = Random.Shared.Next(minMs, maxMs);
            await Task.Delay(delay);
        }
        public async Task<bool> VerifySearchResultsContainAsync(string expectedText)
        {
            _logger.LogInformation($"Verifying search results contain: {expectedText}");
            await SearchResults.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            var content = await SearchResults.TextContentAsync();
            return content?.Contains(expectedText, StringComparison.OrdinalIgnoreCase) ?? false;
            _logger.LogInformation($"Verified search results contain: {expectedText}");
        }

        public async Task ClickFirstResultContainingAsync(string text)
        {
            _logger.LogInformation($"Clicking first result containing: {text}");
            await FirstSearchCareersLink.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            await FirstSearchCareersLink.ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            _logger.LogInformation($"Clicked on link containing: {text}");
        }
       
        public async Task<bool> IsPrometheusResultVisibleAsync()
        {    

            _logger.LogInformation("Checking visibility of Prometheus Group result");
            return await PrometheusSpan.IsVisibleAsync(new() { Timeout = 50000});
        }
    }
}
