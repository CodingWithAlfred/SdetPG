using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIPrometheusJobSearchTest.Utils
{
    public class PlaywrightHelper
    {
        private readonly ILogger _logger;

        public PlaywrightHelper(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<IPlaywright> CreatePlaywrightAsync()
        {
            _logger.LogInformation("Creating Playwright instance");
            return await Playwright.CreateAsync();
        }

        public async Task<IBrowser> LaunchBrowserAsync(IPlaywright playwright, string browserType, bool headless)
        {
            //Args = new[] { "--disable-blink-features=AutomationControlled" },
            // launch browser based with magic arguments to avoid detection for chrome based browsers : Args = new[] { "--disable-blink-features=AutomationControlled" },
            // If the "CI" variable is null, it's local (Headless = false)
            // If the "CI" variable is set, it's the server (Headless = true)

            _logger.LogInformation($"Launching {browserType} browser (headless: {headless})");

            return browserType.ToLower() switch
            {
                "firefox" => await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions  { Headless = headless }),
                "webkit" => await playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = headless }),
                _ => await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Args = new[] { "--disable-blink-features=AutomationControlled" }, Headless = Environment.GetEnvironmentVariable("CI") !=null })
            };
        }

        public async Task<IPage> CreatePageAsync(IBrowser browser)
        {
            _logger.LogInformation("Creating new page");
            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
            });
            return await context.NewPageAsync();
        }
    }
}
