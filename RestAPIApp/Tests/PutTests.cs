using NUnit.Framework;
using RestAPIApp.Models;
using RestAPIApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace RestAPIApp.Tests
{

    [TestFixture]
    public class PutTests
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

            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<ApiClient>();

            var apiSettings = config.GetSection("ApiSettings").Get<ApiSettings>();
            _apiClient = new ApiClient(apiSettings!.BaseUrl, apiSettings.Headers, _logger);
            _logger?.LogInformation("OneTimeSetUp completed. ApiClient initialized.");
        }

        [Test]
        public async Task UpdatePost_WithValidData_ShouldReturnUpdatedPost()
        {
            var updatedPost = new PlaceHolderModel
            {
                UserId = 1,
                Id = 1,
                Title = "Updated Title",
                Body = "Updated Body"
            };

            var result = await _apiClient!.PutAsync<PlaceHolderModel>("/posts/1", updatedPost);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Title, Is.EqualTo(updatedPost.Title));
            Assert.That(result.Body, Is.EqualTo(updatedPost.Body));
            _logger?.LogInformation("Test UpdatePost_WithValidData_ShouldReturnUpdatedPost. Updated post ID: {Id}", result.Id);
        }

        [Test]
        public async Task UpdatePost_PartialUpdate_ShouldSucceed()
        {
            var partialUpdate = new { title = "Only Title Updated" };

            var result = await _apiClient!.PutAsync<PlaceHolderModel>("/posts/1", partialUpdate);

            Assert.That(result, Is.Not.Null);
            _logger?.LogInformation("Test UpdatePost_PartialUpdate_ShouldSucceed. Partially updated post ID: {Id}", result.Id);
        }

        [Test]
        public async Task UpdatePost_WithLargeData_ShouldSucceed()
        {
            var largePost = new PlaceHolderModel
            {
                UserId = 1,
                Id = 1,
                Title = new string('A', 500),
                Body = new string('B', 1000)
            };

            var result = await _apiClient!.PutAsync<PlaceHolderModel>("/posts/1", largePost);

            Assert.That(result, Is.Not.Null);
            _logger?.LogInformation("Test UpdatePost_WithLargeData_ShouldSucceed. Updated post with large data ID: {Id}", result.Id);
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
