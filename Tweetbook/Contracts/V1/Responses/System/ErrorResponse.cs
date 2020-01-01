using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetbook.Contracts.V1.Responses.System
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; }  = new List<ErrorModel>();

        public ErrorResponse()
        {

        }

        public ErrorResponse(ErrorModel error)
        {
            Errors.Add(error);
        }
    }
}
