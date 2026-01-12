using NUnit.Framework;
using RestAPIApp.Models;
using RestAPIApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RestAPIApp.Tests
{
    [TestFixture]
    public class GetTests
    {
        private ApiClient? _apiClient;
        private ILogger<ApiClient>? _logger;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //string s = Directory.GetCurrentDirectory();
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
        public async Task GetAllPosts_ShouldReturnListOfPosts()
        {
            var posts = await _apiClient!.GetAsync<List<PlaceHolderModel>>("/posts");
            Assert.That(posts, Is.Not.Null);
            Assert.That(posts, Has.Count.GreaterThan(0));
            Assert.That(posts![0].Id, Is.GreaterThan(0));
            _logger?.LogInformation("Test GetAllPosts_ShouldReturnListOfPosts. Posts count: {Count}", posts.Count);

        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        public async Task GetPostById_ShouldReturnSinglePost(int postId)
        {
            var post = await _apiClient!.GetAsync<PlaceHolderModel>($"/posts/{postId}");

            Assert.That(post, Is.Not.Null);
            Assert.That(post!.Id, Is.EqualTo(postId));
            Assert.That(post.Title, Is.Not.Null.And.Not.Empty);
            _logger?.LogInformation("TestCase GetPostById_ShouldReturnSinglePost.for ID {Id}. Title: {Title}", postId, post.Title);
        }

        [Test]
        public async Task GetPostsByUserId_ShouldReturnUserPosts()
        {
            var posts = await _apiClient!.GetAsync<List<PlaceHolderModel>>("/posts?userId=1");

            Assert.That(posts, Is.Not.Null);
            Assert.That(posts!.All(p => p.UserId == 1), Is.True);
            _logger?.LogInformation("Test GetPostsByUserId_ShouldReturnUserPosts. Posts count: {Count}", posts.Count);

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
