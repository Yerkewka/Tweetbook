using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests.Posts;
using Tweetbook.Contracts.V1.Responses.Posts;
using Tweetbook.Contracts.V1.Responses.System;
using Tweetbook.Domain;
using Tweetbook.Domain.Post;
using Xunit;

namespace Tweetbook.IntegrationTests
{
    public class PostControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnsEmptyResponse()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Posts.GetAll);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();
            (JsonConvert.DeserializeObject<ApiPagedResponse<PostResponse>>(responseString)).Should().NotBeNull();
        }

        [Fact]
        public async Task Get_ReturnsPost_WhenPostExistsInDatabase()
        {
            // Arrange
            await AuthenticateAsync();
            var createdPost = await CreatePostAsync(new CreatePostRequest { Name = "My test post", Tags = new[] { "test tag" } });

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Posts.Get.Replace("{postId}", createdPost.Data.Id.ToString() ));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedPost = await response.Content.ReadAsAsync<ApiResponse<PostResponse>>();
            returnedPost.Data.Id.Should().Be(createdPost.Data.Id);
            returnedPost.Data.Name.Should().Be("My test post");
        }

        private async Task<ApiResponse<PostResponse>> CreatePostAsync(CreatePostRequest request)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Posts.Create, request);
            return await response.Content.ReadAsAsync<ApiResponse<PostResponse>>();
        }
    }
}
