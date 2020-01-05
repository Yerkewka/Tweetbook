using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests.System.Queries;
using Tweetbook.Contracts.V1.Responses.System;
using Tweetbook.Domain.System;
using Tweetbook.Services.System;

namespace Tweetbook.Helpers
{
    public class PaginationHelpers
    {
        public static ApiPagedResponse<T> Create<T>(string route, IUriService uriService, PaginationFilter paginationFilter, List<T> response)
        {
            var nextPage = paginationFilter.PageNumber >= 1 ? uriService.GetAllObjectsUri(
                    route,
                    new PaginationQuery(paginationFilter.PageNumber + 1, paginationFilter.PageSize)
                ).ToString() : null;
            var previousPage = paginationFilter.PageNumber - 1 >= 1 ? uriService.GetAllObjectsUri(
                    route,
                    new PaginationQuery(paginationFilter.PageNumber - 1, paginationFilter.PageSize)
                ).ToString() : null;

            return new ApiPagedResponse<T>
            {
                Data = response,
                PageNumber = paginationFilter.PageNumber,
                PageSize = paginationFilter.PageSize,
                NextPage = response.Count == paginationFilter.PageSize ? nextPage : null,
                PreviousPage = previousPage
            };
        }
    }
}
