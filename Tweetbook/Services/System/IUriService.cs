using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1.Requests.System.Queries;
using Tweetbook.Domain.System;

namespace Tweetbook.Services.System
{
    public interface IUriService
    {
        Uri GetObjectUri(string route, string prefix, string id);
        Uri GetAllObjectsUri(string route, PaginationQuery pagination);
    }

    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetAllObjectsUri(string route, PaginationQuery pagination)
        {
            var uri = new Uri(_baseUri + route);
            if (pagination == null)
                return uri;

            var modifiedUri = QueryHelpers.AddQueryString(_baseUri + route, "pageNumber", pagination.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());

            return new Uri(modifiedUri);
        }

        public Uri GetObjectUri(string route, string prefix, string id)
        {
            var stringToReplace = "{" + prefix.ToLower() + "Id}";
            return new Uri(_baseUri + route.Replace(stringToReplace, id));
        }
    }
}
