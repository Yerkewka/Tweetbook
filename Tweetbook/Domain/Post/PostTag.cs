using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetbook.Domain.Post
{
    public class PostTag
    {
        public string TagName { get; set; }

        [ForeignKey(nameof(TagName))]
        public virtual Tag Tag { get; set; }

        public Guid PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }
    }
}
