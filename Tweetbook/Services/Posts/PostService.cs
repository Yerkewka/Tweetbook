using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Data;
using Tweetbook.Domain.Post;
using Tweetbook.Domain.Post.Filters;
using Tweetbook.Domain.System;

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
            postToCreate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            await AddNewTags(postToCreate);
            await _dataContext.Posts.AddAsync(postToCreate).ConfigureAwait(false);

            var createdCount = await _dataContext.SaveChangesAsync();
            return createdCount > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);

            if (post == null)
                return false;
            
            _dataContext.Posts.Remove(post);
            var deletedCount = await _dataContext.SaveChangesAsync();
            return deletedCount > 0;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<List<Post>> GetPostsAsync(GetAllPostsFilter filter = null, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Posts.AsQueryable();
            queryable = AddFiltersOnQuery(filter, queryable);

            if (paginationFilter == null)
                return await queryable.Include(x => x.Tags).ToListAsync();

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

            return await queryable
                .Include(x => x.Tags)
                .Skip(skip)
                .Take(paginationFilter.PageSize)
                .ToListAsync();
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            await AddNewTags(postToUpdate);            
            _dataContext.Posts.Update(postToUpdate);
            
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

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _dataContext.Tags.ToListAsync();
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            tag.Name = tag.Name.ToLower();
            var existingTag = await _dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tag.Name);
            if (existingTag != null)
                return true;

            await _dataContext.Tags.AddAsync(tag);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<Tag> GetTagByNameAsync(string tagName)
        {
            return await _dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tagName.ToLower());
        }

        public async Task<bool> DeleteTagAsync(string tagName)
        {
            var tag = await _dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tagName.ToLower());

            if (tag == null)
                return true;

            var postTags = await _dataContext.PostTags.Where(x => x.TagName == tagName.ToLower()).ToListAsync();

            _dataContext.PostTags.RemoveRange(postTags);
            _dataContext.Tags.Remove(tag);
            return await _dataContext.SaveChangesAsync() > postTags.Count;
        }

        private async Task AddNewTags(Post post)
        {
            foreach (var tag in post.Tags)
            {
                var existingTag = await _dataContext.Tags.SingleOrDefaultAsync(x => x.Name == tag.TagName);

                if (existingTag != null)
                    continue;

                await _dataContext.Tags.AddAsync(new Tag
                {
                    Name = tag.TagName,
                    CreationDate = DateTime.UtcNow,
                    CreatorId = post.UserId
                });
            }
        }

        private static IQueryable<Post> AddFiltersOnQuery(GetAllPostsFilter filter, IQueryable<Post> queryable)
        {
            if (!string.IsNullOrEmpty(filter?.UserId))
                queryable = queryable.Where(x => x.UserId == filter.UserId);

            return queryable;
        }
    }
}
