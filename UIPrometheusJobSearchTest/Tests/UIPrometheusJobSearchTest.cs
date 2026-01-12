using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIPrometheusJobSearchTest.Models;
using UIPrometheusJobSearchTest.Pages;
using UIPrometheusJobSearchTest.Utils;

namespace UIPrometheusJobSearchTest.Tests
{

    [TestFixture]
    public class PrometheusJobSearchTests
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IPage? _page;
        private ILogger<PrometheusJobSearchTests>? _logger;
        private TestSettings? _testSettings;
        private PlaywrightHelper? _playwrightHelper;
        private GoogleSearchPage? _googleSearchPage;
        private PrometheusCareersPage? _careersPage;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            // Load configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json")
                .Build();

            // Setup logger
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                //builder.AddDebug();
            });
            _logger = loggerFactory.CreateLogger<PrometheusJobSearchTests>();

            // Load test settings
            _testSettings = config.GetSection("TestSettings").Get<TestSettings>();

            if (_testSettings == null)
            {
                throw new InvalidOperationException("TestSettings could not be loaded from AppSettings.json");
            }

            _logger.LogInformation("Test settings loaded successfully");

            // Initialize Playwright
            _playwrightHelper = new PlaywrightHelper(_logger);
            _playwright = await _playwrightHelper.CreatePlaywrightAsync();
            _browser = await _playwrightHelper.LaunchBrowserAsync(_playwright, _testSettings.BrowserType, _testSettings.Headless);
            _page = await _playwrightHelper.CreatePageAsync(_browser);

            // Set default timeout
            _page.SetDefaultTimeout(_testSettings.DefaultTimeout);

            // Initialize page objects
            _googleSearchPage = new GoogleSearchPage(_page, _logger);
            _careersPage = new PrometheusCareersPage(_page, _logger);

            _logger.LogInformation("OneTimeSetUp completed");
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            _logger?.LogInformation("Starting OneTimeTearDown");

            if (_page != null)
            {
                await _page.CloseAsync();
            }

            if (_browser != null)
            {
                await _browser.CloseAsync();
            }

            _playwright?.Dispose();

            _logger?.LogInformation("OneTimeTearDown completed");
        }

        [Test, Order(1)]
        public async Task Test01_NavigateToGoogleAndSearchPrometheusGroupAndVerify()
        {
            Assert.That(_googleSearchPage, Is.Not.Null, "GoogleSearchPage not initialized");
            Assert.That(_testSettings, Is.Not.Null, "TestSettings not initialized");
            await _googleSearchPage!.NavigateAsync(_testSettings!.BaseUrl);            
            await _googleSearchPage.SearchForAsync(_testSettings.PrometheusSearchTerm);
            bool isResultFound = await _googleSearchPage.IsPrometheusResultVisibleAsync();
            Assert.That(isResultFound, Is.True, "The Prometheus Group search result was not found.");
            _logger?.LogInformation("NavigateToGoogleAndSearch passed: Successfully searched for Prometheus Group");
        }

        [Test, Order(2)]
        public async Task Test02_ClickPrometheusCareerAndNavigateToCareer()
        {
            Assert.That(_googleSearchPage, Is.Not.Null);
            Assert.That(_careersPage, Is.Not.Null);

            await _googleSearchPage!.ClickFirstResultContainingAsync("Careers");
            var navigatedToCareers =   await _careersPage!.VerifyNavigationToCareersAndHandlePopupAsync();
            Assert.That(navigatedToCareers, Is.True, "Did not navigate to Careers page");
            _logger?.LogInformation("ClickPrometheusResultAndNavigateToCareers passed: Successfully navigated to Careers page");
        }

        [Test, Order(3)]
        public async Task Test03_VerifyFourAccordionsHaveTextByTextName()
        {
            Assert.That(_careersPage, Is.Not.Null);
            Assert.That(await _careersPage.VerifyAccordionTextAsync("Who We Serve"), Is.True);
            Assert.That(await _careersPage.VerifyAccordionTextAsync("Solutions"), Is.True);
            Assert.That(await _careersPage.VerifyAccordionTextAsync("Pricing"), Is.True);
            Assert.That(await _careersPage.VerifyAccordionTextAsync("Services"), Is.True);
            //Assert.That(await _careersPage.VerifyAccordionTextAsync("Resources"), Is.True);
            //Assert.That(await _careersPage.VerifyAccordionTextAsync("Company"), Is.True);

            _logger?.LogInformation("VerifyAccordionsExistByAccordionsNameText passed: All accordions have text");
        }

        [Test, Order(4)]
        public async Task Test04_VerifyAccordionsCanOpenAndClose()
        {
            Assert.That(_careersPage, Is.Not.Null);
            Assert.That(_testSettings, Is.Not.Null);

            bool whoWeServeOpened = await _careersPage.ToggleAccordionsAsync("Who We Serve", true);
            Assert.That(whoWeServeOpened, "Who We Serve accordion should be open");
            bool whoWeServeClosed = await _careersPage.ToggleAccordionsAsync("Who We Serve", false);
            Assert.That(whoWeServeClosed, "Who We Serve accordion should be closed");
            _logger?.LogInformation("Test04 passed: Accordions can open and close");
            bool solutionsOpened = await _careersPage.ToggleAccordionsAsync("Solutions", true);
            Assert.That(solutionsOpened, "Solutions accordion should be open");
            bool solutionsClosed = await _careersPage.ToggleAccordionsAsync("Solutions", false);
            Assert.That(solutionsClosed, "Solutions accordion should be closed");
            _logger?.LogInformation("Test04 passed: Accordions can open and close");


            bool servicesOpened = await _careersPage.ToggleAccordionsAsync("Services", true);
            Assert.That(servicesOpened, "Services accordion should be open");
            bool servicesClosed = await _careersPage.ToggleAccordionsAsync("Services", false);
            Assert.That(servicesClosed, "Services accordion should be closed");
            _logger?.LogInformation("Test04 passed: Accordions can open and close");

            bool resourcesOpened = await _careersPage.ToggleAccordionsAsync("Resources", true);
            Assert.That(resourcesOpened, "Resources accordion should be open");
            bool resourcesClosed = await _careersPage.ToggleAccordionsAsync("Resources", false);
            Assert.That(resourcesClosed, "Resources accordion should be closed");
            _logger?.LogInformation("Test04 passed: Accordions can open and close");
            bool companyOpened = await _careersPage.ToggleAccordionsAsync("Company", true);
            Assert.That(companyOpened, "Company accordion should be open");
            bool companyClosed = await _careersPage.ToggleAccordionsAsync("Company", false);
            Assert.That(companyClosed, "Company accordion should be closed");
            _logger?.LogInformation("VerifyAccordionsCanOpenAndClos passed: Accordions can open and close");
        }

        [Test, Order(5)]
        public async Task Test05_VerifySDETJobPostingExists()
        {
            Assert.That(_careersPage, Is.Not.Null);
            Assert.That(_testSettings, Is.Not.Null);

            var jobExists = await _careersPage!.VerifyJobPostingExistsAsync(_testSettings!.ExpectedJobTitle);
            Assert.That(jobExists, Is.True, $"Job posting '{_testSettings.ExpectedJobTitle}' not found");
            _logger?.LogInformation("VerifySDETJobPostingExists passed: SDET job posting found");
        }

        [Test, Order(6)]
        public async Task Test06_ClickOnTheViewAllPrometheusJobs()
        {
            Assert.That(_careersPage, Is.Not.Null);
           Assert.That(await _careersPage!.ClickOnTheViewAllPrometheusJobs(), Is.True);
            _logger?.LogInformation("ClickOnTheViewAllPrometheusJob passed");
        }

    }
}
