using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetbook.Contracts.V1.Responses.System
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }

        public ApiResponse(T data)
        {
            Data = data;
        }
    }
}
