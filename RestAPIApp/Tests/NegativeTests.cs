using NUnit.Framework;
using RestAPIApp.Models;
using RestAPIApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIApp.Tests
{

    [TestFixture]
    public class NegativeTests
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
        public async Task GetPost_WithInvalidId_ShouldReturn404()
        {
            var response = await _apiClient!.GetAsyncRaw("/posts/99999");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            _logger?.LogInformation("Test GetPost_WithInvalidId_ShouldReturn404. Received 404 for non-existent post ID.");
        }

        [Test]
        public async Task GetPost_WithNonExistentEndpoint_ShouldReturn404()
        {
            var response = await _apiClient!.GetAsyncRaw("/invalidendpoint");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            _logger?.LogInformation("Test GetPost_WithNonExistentEndpoint_ShouldReturn404. Received 404 for invalid endpoint.");
        }

        [Test]
        public async Task CreatePost_WithEmptyBody_ShouldStillSucceed()
        {
            var emptyPost = new PlaceHolderModel
            {
                UserId = 1,
                Title = "Title Only",
                Body = ""
            };

            var response = await _apiClient!.PostAsyncRaw("/posts", emptyPost);

            Assert.That(response.IsSuccessStatusCode, Is.True);
            _logger?.LogInformation("Test CreatePost_WithEmptyBody_ShouldStillSucceed. Post created with empty body.");
        }

        [Test]
        public async Task GetPost_WithZeroId_ShouldReturn404()
        {
            var response = await _apiClient!.GetAsyncRaw("/posts/0");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            _logger?.LogInformation("Test GetPost_WithZeroId_ShouldReturn404. Received 404 for post ID 0.");
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
