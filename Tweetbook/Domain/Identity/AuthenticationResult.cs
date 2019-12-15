using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetbook.Domain.Identity
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string RefreshToken { get; internal set; }
    }
}
