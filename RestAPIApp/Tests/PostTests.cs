using NUnit.Framework;
using RestAPIApp.Models;
using RestAPIApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIApp.Tests
{

    [TestFixture]
    public class PostTests
    {
        private ApiClient? _apiClient;
        private ILogger<ApiClient>? _logger;
        private TestDataModel? _testData;

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

            var testDataJson = File.ReadAllText("TestData/TestData.json");
            _testData = JsonSerializer.Deserialize<TestDataModel>(testDataJson);
            _logger?.LogInformation("OneTimeSetUp completed. ApiClient initialized.");
        }

        [Test]
        public async Task CreatePost_WithValidData_ShouldReturnCreatedPost()
        {
            var newPost = new PlaceHolderModel
            {
                UserId = 1,
                Title = "New Test Post",
                Body = "This is a new test post body"
            };

            var createdPost = await _apiClient!.PostAsync<PlaceHolderModel>("/posts", newPost);

            Assert.That(createdPost, Is.Not.Null);
            Assert.That(createdPost!.Title, Is.EqualTo(newPost.Title));
            Assert.That(createdPost.Body, Is.EqualTo(newPost.Body));

            _logger?.LogInformation("Test CreatePost_WithValidData_ShouldReturnCreatedPost. Created post ID: {Id}", createdPost.Id);
        }

        [Test, TestCaseSource(nameof(GetTestPosts))]
        public async Task CreatePost_DataDriven_ShouldSucceed(PlaceHolderModel testPost)
        {
            var createdPost = await _apiClient!.PostAsync<PlaceHolderModel>("/posts", testPost);

            Assert.That(createdPost, Is.Not.Null);
            Assert.That(createdPost.Title, Is.EqualTo(testPost.Title));
            Assert.That(createdPost.UserId, Is.EqualTo(testPost.UserId));

            _logger?.LogInformation("Test CreatePost_DataDriven_ShouldSucceed. Created post ID: {Id}", createdPost.Id);
        }

        [Test]
        public async Task CreatePost_WithMinimalData_ShouldSucceed()
        {
            var minimalPost = new PlaceHolderModel
            {
                UserId = 1,
                Title = "T",
                Body = "B"
            };

            var createdPost = await _apiClient!.PostAsync<PlaceHolderModel>("/posts", minimalPost);

            Assert.That(createdPost, Is.Not.Null);
            Assert.That(createdPost.Title, Is.EqualTo("T"));
            Assert.That(createdPost.UserId, Is.EqualTo(1));

            _logger?.LogInformation("Test CreatePost_WithMinimalData_ShouldSucceed. Created post ID: {Id}", createdPost.Id);

        }

        private static IEnumerable<PlaceHolderModel> GetTestPosts()
        {
            var testDataJson = File.ReadAllText("TestData/TestData.json");
            var testData = JsonSerializer.Deserialize<TestDataModel>(testDataJson);
            return testData?.Posts ?? new List<PlaceHolderModel>();
            
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _logger?.LogInformation("OneTimeTearDown. Resources disposed.");
            _logger = null;
            _apiClient = null;
            _testData = null;
        }
    }
}
