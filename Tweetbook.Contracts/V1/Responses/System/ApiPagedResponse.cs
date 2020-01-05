using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetbook.Contracts.V1.Responses.System
{
    public class ApiPagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string NextPage { get; set; }
        public string PreviousPage { get; set; }

        public ApiPagedResponse() {}

        public ApiPagedResponse(IEnumerable<T> data)
        {
            Data = data;
        }
    }
}
