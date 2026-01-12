using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace UIPrometheusJobSearchTest.Pages
{

    public class PrometheusCareersPage
    {
        private readonly IPage _page;
        private readonly ILogger _logger;

        private ILocator FirstButtonInCareerPage => _page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "\r\n            View Open Prometheus Jobs\r\n            " }).First;


        private ILocator ClosePopupButton => _page.GetByRole(AriaRole.Button, new() { Name = "Close" });
        private ILocator WhoWeServeAccordion => _page.GetByText("Who We Serve", new() { Exact = true }).First;

        private ILocator SolutionsAccordion => _page.GetByText("Solutions", new() { Exact = true }).First;

        private ILocator PricingLink => _page.GetByText("Pricing", new() { Exact = true }).First;

        private ILocator ServicesAccordion => _page.GetByText("Services", new() { Exact = true }).First;

        private ILocator ResourcesAccordion => _page.GetByText("Resources", new() { Exact = true }).First;

        private ILocator CompanyAccordion => _page.GetByText("Company", new() { Exact = true }).First;

        private ILocator ViewallPrometheusJobsonLinkedInButton => _page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "View all Prometheus Jobs on LinkedIn" }).First;


        public PrometheusCareersPage(IPage page, ILogger logger)
        {
            _page = page;
            _logger = logger;
        }

        public async Task<bool> VerifyNavigationToCareersAndHandlePopupAsync()
        {

            _logger.LogInformation("Verifying navigation to Careers page");

            await CloseConferencePopupIfPresentAsync();


            await FirstButtonInCareerPage.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 5000
            });

            _logger.LogInformation($"Current URL: {_page.Url}");

            return _page.Url.Contains("career", StringComparison.OrdinalIgnoreCase);
        }
        private async Task CloseConferencePopupIfPresentAsync(int maxWaitMs = 6000)
        {
            _logger.LogInformation("Waiting for popup (if any) before sending ESC.");

            var elapsed = 0;
            const int pollInterval = 500;

            while (elapsed < maxWaitMs)
            {
                if (await ClosePopupButton.CountAsync() > 0)
                {
                    _logger.LogInformation("Popup detected. Sending ESC key.");
                    await _page.Keyboard.PressAsync("Escape");
                    return;
                }

                await _page.WaitForTimeoutAsync(pollInterval);
                elapsed += pollInterval;
            }


            _logger.LogInformation("Popup not detected. Sending ESC anyway.");
            await _page.Keyboard.PressAsync("Escape");
        }

        public async Task<bool> VerifyJobPostingExistsAsync(string jobTitle)
        {
            _logger.LogInformation("Starting targeted search for SDET job link.");
            // Move to the section Open Positions to begin the search
            var openPositionsHeader = _page.GetByRole(AriaRole.Heading, new() { Name = "Open Positions" });
            await openPositionsHeader.ScrollIntoViewIfNeededAsync();

            //Define the exact locator text we are searching for
            var sdetLink = _page.GetByRole(AriaRole.Link, new()
            {
                Name = jobTitle,
                Exact = true
            });          

            int maxScrolls = 50;
            for (int i = 0; i < maxScrolls; i++)
            {
                if (await sdetLink.IsVisibleAsync())
                {
                    await sdetLink.ScrollIntoViewIfNeededAsync();
                    return true;
                }

                await _page.EvaluateAsync("window.scrollBy(0, window.innerHeight)");
                await _page.WaitForTimeoutAsync(300);
            }
            return false;
        }
        public async Task<bool> VerifyAccordionTextAsync(string accordionName)
        {
            if (string.IsNullOrWhiteSpace(accordionName))
                throw new ArgumentException("Accordion name cannot be null or empty.");

            ILocator targetMenu = accordionName.ToLower() switch
            {
                "who we serve" => WhoWeServeAccordion,
                "solutions" => SolutionsAccordion,
                "pricing" => PricingLink,
                "services" => ServicesAccordion,
                "resources" => ResourcesAccordion,
                "company" => CompanyAccordion,
                _ => throw new ArgumentException($"Menu '{accordionName}' not defined.")
            };

            var element = targetMenu.First;

            await element.WaitForAsync(new()
            {
                State = WaitForSelectorState.Visible,
                Timeout = 5000
            });

            var actualText = (await element.InnerTextAsync())
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault()
                    ?.Trim();

            return actualText!.Equals(accordionName, StringComparison.OrdinalIgnoreCase);
        }


        public async Task<bool> ToggleAccordionsAsync(string accordionName, bool shouldOpen)
        {
            if (string.IsNullOrWhiteSpace(accordionName))
                throw new ArgumentException("Accordion name cannot be null or empty.");

            ILocator targetMenu = accordionName.ToLower() switch
            {
                "who we serve" => WhoWeServeAccordion,
                "solutions" => SolutionsAccordion,
                "pricing" => PricingLink,
                "services" => ServicesAccordion,
                "resources" => ResourcesAccordion,
                "company" => CompanyAccordion,
                _ => throw new ArgumentException($"Accordion '{accordionName}' not defined.")
            };

            // Get the accordionName items inside this menu
            var dropdownItems = targetMenu.Locator(".dropdown-container > .dropdown-item");

            if (shouldOpen)
            {
                // Hover menu to open
                await targetMenu.HoverAsync(new() { Force = true });
                await _page.WaitForTimeoutAsync(200);
                return true;
            
            }
            else
            {   // Close: hover body or click outside
                await _page.Mouse.ClickAsync(0, 0);
                await _page.WaitForTimeoutAsync(200);
                return true;
                

            }
           
        }

        internal async Task<bool?> ClickOnTheViewAllPrometheusJobs()
        {   
            _logger.LogInformation("Click View all Prometheus Jobs on LinkedIn Button");
            await ViewallPrometheusJobsonLinkedInButton.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 5000
            });
            await ViewallPrometheusJobsonLinkedInButton.ClickAsync();
            await _page.WaitForTimeoutAsync(200);
            return true;
        }
    }

    }
