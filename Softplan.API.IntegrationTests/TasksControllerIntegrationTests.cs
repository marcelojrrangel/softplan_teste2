using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace Softplan.API.IntegrationTests
{
    public class TasksControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TasksControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // Remove authorization for tests
                    services.AddSingleton<IAuthorizationHandler, AllowAnonymousAuthorizationHandler>();
                });
            }).CreateClient();
        }

        private class AllowAnonymousAuthorizationHandler : IAuthorizationHandler
        {
            public Task HandleAsync(AuthorizationHandlerContext context)
            {
                foreach (var requirement in context.Requirements)
                {
                    context.Succeed(requirement);
                }
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async Task PostTask_CreatesAndReturnsTask()
        {
            // Arrange
            var request = new { Title = "Integration Test Task", Description = "Test Description", UserId = "integrationuser" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1.0/tasks", request);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdTask = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            Assert.NotNull(createdTask);
            Assert.Equal("Integration Test Task", createdTask["title"]?.ToString());
        }

        [Fact]
        public async Task GetTasks_ReturnsTasksForUser()
        {
            // Arrange
            var request = new { Title = "Another Integration Test Task", Description = "Test Description 2", UserId = "integrationuser2" };
            await _client.PostAsJsonAsync("/api/v1.0/tasks", request);

            // Act
            var response = await _client.GetAsync("/api/v1.0/tasks/integrationuser2");

            // Assert
            response.EnsureSuccessStatusCode();
            var tasks = await response.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>();
            Assert.NotNull(tasks);
            Assert.Single(tasks);
            Assert.Equal("Another Integration Test Task", tasks[0]["title"]?.ToString());
        }
        
        [Fact]
        public async Task PutCompleteTask_CompletesTask()
        {
            // Arrange
            var request = new { Title = "Completable Task", Description = "Test Description 3", UserId = "integrationuser3" };
            var postResponse = await _client.PostAsJsonAsync("/api/v1.0/tasks", request);
            var createdTask = await postResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            Assert.NotNull(createdTask);
            var id = ((JsonElement)createdTask["id"]).GetInt32();

            // Act
            var putResponse = await _client.PutAsync($"/api/v1.0/tasks/{id}/complete", null);

            // Assert
            putResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteTask_DeletesTask()
        {
            // Arrange
            var request = new { Title = "Deletable Task", Description = "Test Description 4", UserId = "integrationuser4" };
            var postResponse = await _client.PostAsJsonAsync("/api/v1.0/tasks", request);
            var createdTask = await postResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            Assert.NotNull(createdTask);
            var id = ((JsonElement)createdTask["id"]).GetInt32();

            // Act
            var deleteResponse = await _client.DeleteAsync($"/api/v1.0/tasks/{id}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
