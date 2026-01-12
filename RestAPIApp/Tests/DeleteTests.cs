using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestAPIApp.Models;
using RestAPIApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIApp.Tests
{

    [TestFixture]
    public class DeleteTests
    {
        private ApiClient? _apiClient;
        private ILogger<ApiClient>? _logger;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json")
                .Build();

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
            _logger = loggerFactory.CreateLogger<ApiClient>();

            var apiSettings = config.GetSection("ApiSettings").Get<ApiSettings>();
            _apiClient = new ApiClient(apiSettings!.BaseUrl, apiSettings.Headers, _logger);
            _logger?.LogInformation("OneTimeSetUp completed. ApiClient initialized.");
        }

        [Test]
        public async Task DeletePost_WithValidId_ShouldReturnSuccess()
        {
            var response = await _apiClient!.DeleteAsync("/posts/1");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            _logger?.LogInformation("Test DeletePost_WithValidId_ShouldReturnSuccess. Deleted post with ID 1.");
        }

        [Test]
        public async Task DeletePost_MultipleIds_ShouldSucceed()
        {
            var response1 = await _apiClient!.DeleteAsync("/posts/1");
            var response2 = await _apiClient!.DeleteAsync("/posts/2");
            var response3 = await _apiClient!.DeleteAsync("/posts/3");

            Assert.That(response1.IsSuccessStatusCode, Is.True);
            Assert.That(response2.IsSuccessStatusCode, Is.True);
            Assert.That(response3.IsSuccessStatusCode, Is.True);
            _logger?.LogInformation("Test DeletePost_MultipleIds_ShouldSucceed. Deleted posts with IDs 1, 2, and 3.");
        }

        [Test]
        public async Task DeletePost_HighId_ShouldReturnSuccess()
        {
            var response = await _apiClient!.DeleteAsync("/posts/100");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            _logger?.LogInformation("Test DeletePost_HighId_ShouldReturnSuccess. Deleted post with ID 100.");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _logger?.LogInformation("OneTimeTearDown. Resources disposed.");
            _logger = null;
            _apiClient = null;
        }
    }
}
