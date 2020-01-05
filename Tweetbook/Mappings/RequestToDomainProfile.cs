using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1.Requests.Posts.Queries;
using Tweetbook.Contracts.V1.Requests.System.Queries;
using Tweetbook.Domain.Post.Filters;
using Tweetbook.Domain.System;

namespace Tweetbook.Mappings
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<GetAllPostsQuery, GetAllPostsFilter>();
        }
    }
}
