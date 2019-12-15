using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Data;
using Tweetbook.Domain;

namespace Tweetbook.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreatePostAsync(Post postToCreate)
        {
            await _dataContext.Posts.AddAsync(postToCreate).ConfigureAwait(false);
            var createdCount = await _dataContext.SaveChangesAsync();

            return createdCount > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);
            _dataContext.Posts.Remove(post);
            var deletedCount = await _dataContext.SaveChangesAsync();
            
            return deletedCount > 0;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _dataContext.Posts.ToListAsync();
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            var post = await GetPostByIdAsync(postToUpdate.Id);
            _dataContext.Posts.Update(post);
            var updatedCount = await _dataContext.SaveChangesAsync();
            
            return updatedCount > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);

            if (post == null)
                return false;

            if (post.UserId != userId)
                return false;

            return true;
        }
    }
}
