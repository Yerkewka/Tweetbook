using Refit;
using System;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1.Requests.Indentity;
using Tweetbook.Contracts.V1.Requests.Posts;

namespace Tweetbook.Sdk.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cachedToken = string.Empty;

            var identityApi = RestService.For<IIdentityApi>("https://localhost:5001");
            var tweetbookApi = RestService.For<ITweetbookApi>("https://localhost:5001", new RefitSettings { 
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
            {
                Email = "sdkaccount@gmail.com",
                Password = "Test123!"
            });

            var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
            {
                Email = "sdkaccount@gmail.com",
                Password = "Test123!"
            });

            cachedToken = loginResponse.Content.Token;

            var allPosts = await tweetbookApi.GetAllAsync();

            var createdPost = await tweetbookApi.CreateAsync(new CreatePostRequest
            {
                Name = "This is the new post created by SDK",
                Tags = new[] { "sdk" }
            });

            var retrievedPost = await tweetbookApi.GetAsync(createdPost.Content.Id);

            var updatedPost = await tweetbookApi.UpdateAsync(createdPost.Content.Id, new UpdatePostRequest
            {
                Name = "This is the updated post by SDK"
            });

            var deletedPost = await tweetbookApi.DeleteAsync(createdPost.Content.Id);
        }
    }
}
