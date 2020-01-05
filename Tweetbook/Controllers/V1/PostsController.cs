using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Cache;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests.Posts;
using Tweetbook.Contracts.V1.Requests.Posts.Queries;
using Tweetbook.Contracts.V1.Requests.System.Queries;
using Tweetbook.Contracts.V1.Responses.Posts;
using Tweetbook.Contracts.V1.Responses.System;
using Tweetbook.Domain.Post;
using Tweetbook.Domain.Post.Filters;
using Tweetbook.Domain.System;
using Tweetbook.Extensions;
using Tweetbook.Helpers;
using Tweetbook.Services.Posts;
using Tweetbook.Services.System;

namespace Tweetbook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public PostsController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        [Cached(600)]
        public async Task<IActionResult> GetAll([FromQuery]GetAllPostsQuery query, [FromQuery]PaginationQuery paginationQuery)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllPostsFilter>(query);

            var posts = await _postService.GetPostsAsync(filter, paginationFilter);
            var postsResponse = _mapper.Map<List<PostResponse>>(posts);
            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
                return Ok(new ApiPagedResponse<PostResponse>(postsResponse));
            
            return Ok(PaginationHelpers.Create(ApiRoutes.Posts.GetAll, _uriService, paginationFilter, postsResponse));
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        [Cached(600)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
                return NotFound();

            return Ok(new ApiResponse<PostResponse>(_mapper.Map<PostResponse>(post)));
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var postId = Guid.NewGuid();
            var post = new Post { 
                Id = postId,
                Name = postRequest.Name, 
                UserId = HttpContext.GetUserId(),
                Tags = postRequest.Tags.Select(x => new PostTag { PostId = postId, TagName = x }).ToList() 
            };

            await _postService.CreatePostAsync(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var locationUrl = $"{baseUrl}/{ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString())}";

            return Created(locationUrl, new ApiResponse<PostResponse>(_mapper.Map<PostResponse>(post)));
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new
                {
                    error = "You do not own this post"
                });

            var post = await _postService.GetPostByIdAsync(postId);
            post.Name = request.Name;

            var isUpdated = await _postService.UpdatePostAsync(post);
            if (!isUpdated)
                return NotFound();

            return Ok(new ApiResponse<PostResponse>(_mapper.Map<PostResponse>(post)));
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new
                {
                    error = "You do not own this post"
                });

            var isDeleted = await _postService.DeletePostAsync(postId);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
