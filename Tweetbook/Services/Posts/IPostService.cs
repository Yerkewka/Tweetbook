﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Domain.Post;
using Tweetbook.Domain.Post.Filters;
using Tweetbook.Domain.System;

namespace Tweetbook.Services.Posts
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync(GetAllPostsFilter filter = null, PaginationFilter paginationFilter = null);
        Task<Post> GetPostByIdAsync(Guid postId);
        Task<bool> CreatePostAsync(Post postToCreate);
        Task<bool> UpdatePostAsync(Post postToUpdate);
        Task<bool> DeletePostAsync(Guid postId);
        Task<bool> UserOwnsPostAsync(Guid postId, string userId);
        Task<List<Tag>> GetAllTagsAsync();
        Task<bool> CreateTagAsync(Tag tag);
        Task<Tag> GetTagByNameAsync(string tagName);
        Task<bool> DeleteTagAsync(string tagName);
    }
}
